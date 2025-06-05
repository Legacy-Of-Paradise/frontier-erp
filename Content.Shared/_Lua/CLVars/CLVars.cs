// LuaWorld - This file is licensed under AGPLv3
// Copyright (c) 2025 LuaWorld
// See AGPLv3.txt for details.

using Robust.Shared.Configuration;

namespace Content.Shared._Lua.CLVars
{
    [CVarDefs]
    public sealed partial class CLVars
    {
        /// <summary>
        /// Интервал автоматической выдачи зарплаты в секундах 3600 = 1 час.
        /// </summary>
        public static readonly CVarDef<float> AutoSalaryInterval =
            CVarDef.Create("salary.auto_interval", 3600f, CVar.SERVER | CVar.ARCHIVE);
    }
}
