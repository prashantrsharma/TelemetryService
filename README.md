# TelemetryService

A dependency is an external component that is called by your app. Use TelemetryService to log details about the dependencies.
Usage
TelemetryClientService.LogDependency(async () => await GetDataAsync(), appInsights, "HTTPGET", "Google", "data");


