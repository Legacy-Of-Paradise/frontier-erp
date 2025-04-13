buildAllDebug
    Builds all projects with debug configuration // Выполняет сборку всех проектов в конфигурации DEBUG
buildAllRelease
    Builds all projects with release configuration // Выполняет сборку всех проектов в конфигурации RELEASE
buildAllTools
    Builds all projects with tools configuration // Выполняет сборку всех проектов в конфигурации TOOLS

The debug vs release build is simply what people develop in vs the actual server.
The release build contains various optimizations, while the debug build contains debugging tools.
If you're mapping, use the release or tools build as it will run smoother with less crashes.
//
DEBUG предназначен для разработки и отладки, RELEASE для работы в режиме реального сервера.
В RELEASE-сборке используются различные оптимизации, а в DEBUG-сборке - инструменты отладки.
Для маппинга используйте RELEASE или TOOLS.


runQuickAll
    Runs the client and server without building	//Запускает приложения клиента и сервера без сборки
runQuickClient
    Runs the client without building //Запускает приложение только клиента без сборки
runQuickServer
    Runs the server without building //Запускает приложение только сервера без сборки

runTests
    Runs the unit tests, makes sure various C# systems work as intended
runTestsIntegration
    Runs the integration tests, makes sure various C# systems work as intended
runTestsYAML
    Runs the YAML linter and finds issues with the YAML files that you probably wouldn't otherwise
