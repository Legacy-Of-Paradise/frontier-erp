using System.Linq;
using System.Text;
using Content.Client.Materials;
using Content.Shared._NF.Lathe;
using Content.Shared._NF.Research.Prototypes;
using Content.Shared.Lathe;
using Content.Shared.Research.Prototypes;
using Robust.Client.AutoGenerated;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;

namespace Content.Client._NF.Lathe.UI;

/// <summary>
/// Main window for the blueprint lathe.
/// Works like the lathe menu, though it selects blueprints by category
/// and allows multiple recipe selection on the same blueprint.
/// </summary>
[GenerateTypedNameReferences]
public sealed partial class BlueprintLatheNFMenu : DefaultWindow
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    private readonly SpriteSystem _spriteSystem;
    private readonly BlueprintLatheSystem _lathe;
    private readonly MaterialStorageSystem _materialStorage;

    public event Action<BaseButton.ButtonEventArgs>? OnServerListButtonPressed;
    public event Action<ProtoId<BlueprintPrototype>, int[], int>? RecipeQueueAction;
    public event Action<int>? QueueDeleteAction;
    public event Action<int>? QueueMoveUpAction;
    public event Action<int>? QueueMoveDownAction;
    public event Action? DeleteFabricatingAction;

    public Dictionary<ProtoId<BlueprintPrototype>, int[]> RecipesByBlueprintType = new();

    public List<ProtoId<BlueprintPrototype>>? BlueprintTypes;

    public EntityUid Entity;

    public BlueprintLatheNFMenu()
    {
        RobustXamlLoader.Load(this);
        IoCManager.InjectDependencies(this);

        _spriteSystem = _entityManager.System<SpriteSystem>();
        _lathe = _entityManager.System<BlueprintLatheSystem>();
        _materialStorage = _entityManager.System<MaterialStorageSystem>();

        SearchBar.OnTextChanged += _ =>
        {
            PopulateRecipes();
        };
        AmountLineEdit.OnTextChanged += _ =>
        {
            PopulateRecipes();
        };

        BlueprintTypeOption.OnItemSelected += OnItemSelected;

        ServerListButton.OnPressed += a => OnServerListButtonPressed?.Invoke(a);
        DeleteFabricating.OnPressed += _ => DeleteFabricatingAction?.Invoke();
        DeleteFabricating.AddStyleClass("OpenLeft");
        AllButton.OnPressed += _ => SetAllRecipesPressed(true);
        ClearButton.OnPressed += _ => SetAllRecipesPressed(false);
        PrintButton.OnPressed += _ => QueueBlueprint();
    }

    public void SetEntity(EntityUid uid)
    {
        Entity = uid;

        MaterialsList.SetOwner(Entity);
    }

    protected override void Opened()
    {
        base.Opened();

        if (_entityManager.TryGetComponent<BlueprintLatheComponent>(Entity, out var latheComp))
            AmountLineEdit.SetText(latheComp.DefaultProductionAmount.ToString());
    }

    /// <summary>
    /// Populates the list of all the recipes
    /// </summary>
    public void PopulateRecipes()
    {
        List<(LatheRecipePrototype recipe, int index)> recipesToShow = new();

        RecipeList.Children.Clear();

        if (!_entityManager.TryGetComponent<BlueprintLatheClientStateComponent>(Entity, out var clientLathe))
            return;

        // Coerce a null blueprint type into a valid one if possible.
        int[]? recipeBitset = null;
        if (clientLathe.CurrentBlueprintType == null
            || !RecipesByBlueprintType.TryGetValue(clientLathe.CurrentBlueprintType.Value, out recipeBitset))
        {
            clientLathe.CurrentBlueprintType = RecipesByBlueprintType.Keys.FirstOrNull();
            clientLathe.CurrentRecipes = null;
        }

        // Check that we can still get a set of recipes from what we have.
        if (clientLathe.CurrentBlueprintType == null
            || !RecipesByBlueprintType.TryGetValue(clientLathe.CurrentBlueprintType.Value, out recipeBitset)
            || !_lathe.PrintableRecipesByType.TryGetValue(clientLathe.CurrentBlueprintType.Value, out var recipeList))
        {
            return;
        }

        // Find bits in the bitset, add recipes for our current blueprint type.
        for (int i = 0; i < recipeBitset.Length; i++)
        {
            for (int j = 0; j < 32; j++)
            {
                var index = 32 * i + j;
                if (index >= recipeList.Count)
                    break;

                // No bit or no recipe?
                if ((recipeBitset[i] & (1 << j)) == 0
                    || !_prototypeManager.TryIndex(recipeList[index], out var recipe))
                {
                    continue;
                }

                if (SearchBar.Text.Trim().Length != 0)
                {
                    if (_lathe.GetRecipeName(recipe).ToLowerInvariant().Contains(SearchBar.Text.Trim().ToLowerInvariant()))
                        recipesToShow.Add((recipe, index));
                }
                else
                {
                    recipesToShow.Add((recipe, index));
                }
            }
        }

        if (!int.TryParse(AmountLineEdit.Text, out var quantity) || quantity <= 0)
            quantity = 1;

        var sortedRecipesToShow = recipesToShow.OrderBy(x => _lathe.GetRecipeName(x.recipe));
        if (!_entityManager.TryGetComponent(Entity, out BlueprintLatheComponent? lathe))
            return;

        foreach (var item in sortedRecipesToShow)
        {
            // TODO: fix lookup by blueprint type
            var canProduce = _lathe.CanProduceRecipe(Entity, clientLathe.CurrentBlueprintType.Value, item.recipe, quantity, component: lathe);

            var control = new BlueprintRecipeControl(_lathe, item.recipe, () => GenerateTooltipText(item.recipe, lathe), canProduce, GetRecipeDisplayControl(item.recipe), item.index);

            var idx = item.index / 32;
            var val = 1 << (item.index % 32);
            if (clientLathe.CurrentRecipes != null
                && clientLathe.CurrentRecipes.Length > idx
                && (clientLathe.CurrentRecipes[idx] & val) != 0)
            {
                control.SetSelected(true);
            }

            control.OnSelectedAction += selected => SetRecipeSelected(item.index, selected);

            RecipeList.AddChild(control);
        }
    }

    private string GenerateTooltipText(LatheRecipePrototype prototype, BlueprintLatheComponent component)
    {
        StringBuilder sb = new();

        foreach (var (id, amount) in component.BlueprintPrintMaterials)
        {
            if (!_prototypeManager.TryIndex(id, out var proto))
                continue;

            var adjustedAmount = SharedLatheSystem.AdjustMaterial(amount, prototype.ApplyMaterialDiscount, component.FinalMaterialUseMultiplier);
            var sheetVolume = _materialStorage.GetSheetVolume(proto);

            var unit = Loc.GetString(proto.Unit);
            var sheets = adjustedAmount / (float)sheetVolume;

            var availableAmount = _materialStorage.GetMaterialAmount(Entity, id);
            var missingAmount = Math.Max(0, adjustedAmount - availableAmount);
            var missingSheets = missingAmount / (float)sheetVolume;

            var name = Loc.GetString(proto.Name);

            string tooltipText;
            if (missingSheets > 0)
            {
                tooltipText = Loc.GetString("lathe-menu-material-amount-missing", ("amount", sheets), ("missingAmount", missingSheets), ("unit", unit), ("material", name));
            }
            else
            {
                var amountText = Loc.GetString("lathe-menu-material-amount", ("amount", sheets), ("unit", unit));
                tooltipText = Loc.GetString("lathe-menu-tooltip-display", ("material", name), ("amount", amountText));
            }

            sb.AppendLine(tooltipText);
        }

        var desc = _lathe.GetRecipeDescription(prototype);
        if (!string.IsNullOrWhiteSpace(desc))
            sb.AppendLine(Loc.GetString("lathe-menu-description-display", ("description", desc)));

        // Remove last newline
        if (sb.Length > 0)
            sb.Remove(sb.Length - 1, 1);

        return sb.ToString();
    }

    public void UpdateCategories()
    {
        ProtoId<BlueprintPrototype>? selectedCategory = null;
        if (_entityManager.TryGetComponent<BlueprintLatheClientStateComponent>(Entity, out var clientLathe))
            selectedCategory = clientLathe.CurrentBlueprintType;

        // Get categories from recipe dictionary.
        List<ProtoId<BlueprintPrototype>> currentBlueprintTypes = RecipesByBlueprintType.Keys.ToList();

        if (BlueprintTypes != null
            && (BlueprintTypes.Count == currentBlueprintTypes.Count || !BlueprintTypes.All(currentBlueprintTypes.Contains)))
        {
            return;
        }

        BlueprintTypes = currentBlueprintTypes;
        var sortedBlueprintTypes = currentBlueprintTypes
            .Select(p => _prototypeManager.Index(p))
            .OrderBy(p => Loc.GetString(p.Name))
            .ToList();

        BlueprintTypeOption.Clear();
        var selectedIndex = false;
        foreach (var blueprintType in sortedBlueprintTypes)
        {
            var index = BlueprintTypes.IndexOf(blueprintType.ID);
            BlueprintTypeOption.AddItem(Loc.GetString(blueprintType.Name), index);
            if (blueprintType == selectedCategory)
            {
                BlueprintTypeOption.SelectId(index);
                selectedIndex = true;
            }
        }

        // Default to whatever the first item is.  No blueprint type is not a valid selection.
        if (!selectedIndex)
            BlueprintTypeOption.SelectId(0);
    }

    /// <summary>
    /// Populates the build queue list with all queued items
    /// </summary>
    /// <param name="queue"></param>
    public void PopulateQueueList(List<BlueprintLatheRecipeBatch> queue)
    {
        QueueList.DisposeAllChildren();

        var idx = 1;
        foreach (var batch in queue)
        {
            string displayText;
            if (batch.ItemsRequested > 1)
                displayText = $"{idx}. {GetBlueprintName(batch.BlueprintType)} ({batch.ItemsPrinted}/{batch.ItemsRequested})";
            else
                displayText = $"{idx}. {GetBlueprintName(batch.BlueprintType)}";

            var queuedRecipeBox = new QueuedRecipeControl(displayText, idx - 1, GetBlueprintDisplayControl(batch.BlueprintType));
            queuedRecipeBox.OnDeletePressed += s => QueueDeleteAction?.Invoke(s);
            queuedRecipeBox.OnMoveUpPressed += s => QueueMoveUpAction?.Invoke(s);
            queuedRecipeBox.OnMoveDownPressed += s => QueueMoveDownAction?.Invoke(s);
            QueueList.AddChild(queuedRecipeBox);
            idx++;
        }
    }

    public void SetQueueInfo(ProtoId<BlueprintPrototype>? recipe)
    {
        FabricatingContainer.Visible = recipe != null;
        if (recipe == null)
            return;

        FabricatingDisplayContainer.Children.Clear();
        FabricatingDisplayContainer.AddChild(GetBlueprintDisplayControl(recipe.Value));

        NameLabel.Text = GetBlueprintName(recipe);
    }

    public Control GetBlueprintDisplayControl(ProtoId<BlueprintPrototype> recipe)
    {
        if (_prototypeManager.TryIndex(recipe, out var blueprintPrototype))
        {
            var entProtoView = new EntityPrototypeView();
            entProtoView.SetPrototype(blueprintPrototype.Blueprint);
            return entProtoView;
        }

        return new Control();
    }

    public Control GetRecipeDisplayControl(LatheRecipePrototype recipe)
    {
        if (recipe.Icon != null)
        {
            var textRect = new TextureRect();
            textRect.Texture = _spriteSystem.Frame0(recipe.Icon);
            return textRect;
        }

        if (recipe.Result is { } result)
        {
            var entProtoView = new EntityPrototypeView();
            entProtoView.SetPrototype(result);
            return entProtoView;
        }

        return new Control();
    }

    private void OnItemSelected(OptionButton.ItemSelectedEventArgs obj)
    {
        if (!_entityManager.TryGetComponent<BlueprintLatheClientStateComponent>(Entity, out var clientLathe))
        {
            PopulateRecipes();
            return;
        }

        BlueprintTypeOption.SelectId(obj.Id);
        if (obj.Id == -1)
        {
            if (clientLathe.CurrentBlueprintType != null)
            {
                clientLathe.CurrentBlueprintType = null;
                clientLathe.CurrentRecipes = null;
            }
        }
        else
        {
            if (clientLathe.CurrentBlueprintType != BlueprintTypes?[obj.Id])
            {
                clientLathe.CurrentBlueprintType = BlueprintTypes?[obj.Id];
                if (clientLathe.CurrentBlueprintType != null
                    && RecipesByBlueprintType.TryGetValue(clientLathe.CurrentBlueprintType.Value, out var recipes))
                {
                    // NOTE: potentially buggy if items hot-reloaded and recipes added/lost
                    clientLathe.CurrentRecipes = new int[recipes.Length];
                }
                else
                {
                    clientLathe.CurrentRecipes = null;
                }
            }
        }

        PopulateRecipes();
    }

    private string GetBlueprintName(ProtoId<BlueprintPrototype>? recipe)
    {
        if (recipe != null
            && _prototypeManager.TryIndex(recipe, out var blueprintPrototype)
            && _prototypeManager.TryIndex(blueprintPrototype.Blueprint, out var entityPrototype))
        {
            return entityPrototype.Name;
        }

        return Loc.GetString("blueprint-lathe-menu-default-blueprint-name");
    }

    private void QueueBlueprint()
    {
        if (!_entityManager.TryGetComponent<BlueprintLatheClientStateComponent>(Entity, out var clientLathe))
            return;

        if (clientLathe.CurrentBlueprintType == null
            || clientLathe.CurrentRecipes == null
            || !RecipesByBlueprintType.TryGetValue(clientLathe.CurrentBlueprintType.Value, out var recipeSet))
        {
            return;
        }

        if (!int.TryParse(AmountLineEdit.Text, out var quantity) || quantity <= 0)
            quantity = 1;

        RecipeQueueAction?.Invoke(clientLathe.CurrentBlueprintType.Value, clientLathe.CurrentRecipes, quantity);
    }

    private void SetAllRecipesPressed(bool pressed)
    {
        foreach (var child in RecipeList.Children)
        {
            if (child is not BlueprintRecipeControl blueprint)
                continue;

            blueprint.SetSelected(pressed);
        }
    }

    private void SetRecipeSelected(int index, bool selected)
    {
        if (!_entityManager.TryGetComponent<BlueprintLatheClientStateComponent>(Entity, out var clientLathe))
            return;

        var intIndex = index / 32;
        var val = 1 << (index % 32);
        if (clientLathe.CurrentRecipes != null
            && intIndex < clientLathe.CurrentRecipes.Length)
        {
            if (selected)
                clientLathe.CurrentRecipes[intIndex] |= val;
            else
                clientLathe.CurrentRecipes[intIndex] &= ~val;
        }
    }
}
