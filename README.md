# ref-dontet-otel
# Auto instrumentation of a .NET binary on linux machine

Initial setup on a host that would run the .NET executable:
===========================================================
Follow the steps outlined here: https://github.com/open-telemetry/opentelemetry-dotnet-instrumentation#shell-scripts
```
# Download the bash script
curl -sSfL https://raw.githubusercontent.com/open-telemetry/opentelemetry-dotnet-instrumentation/v1.0.0-rc.2/otel-dotnet-auto-install.sh -O

# Install core files
sh ./otel-dotnet-auto-install.sh

# Enable execution for the instrumentation script
chmod +x $HOME/.otel-dotnet-auto/instrument.sh

# Setup the instrumentation for the current shell session
. $HOME/.otel-dotnet-auto/instrument.sh
```

Terminal 1:
===========
// Run the complete OpenTelemetry setup
```
cd Configs 
docker-compose up
```

Terminal 2:
===========
// Build and run the app
```
cd application/SampleOpenTelemetry
dotnet clean && dotnet publish -r linux-x64 --self-contained false

cd bin/Debug/net7.0/linux-x64/publish/

# For using OTLP gRPC to export telemetry data to collector
OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4317 OTEL_EXPORTER_OTLP_PROTOCOL=grpc OTEL_DOTNET_AUTO_INSTRUMENTATION_ENABLED=true OTEL_TRACES_EXPORTER=otlp OTEL_METRICS_EXPORTER=otlp OTEL_LOGS_EXPORTER=otlp OTEL_SERVICE_NAME=myapp OTEL_RESOURCE_ATTRIBUTES=deployment.environment=staging,service.version=1.0.0 ./SampleOpenTelemetry 

# For using OTLP HTTP to export telemetry data to collector
OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4318 OTEL_EXPORTER_OTLP_PROTOCOL=http/protobuf OTEL_DOTNET_AUTO_INSTRUMENTATION_ENABLED=true OTEL_TRACES_EXPORTER=otlp OTEL_METRICS_EXPORTER=otlp OTEL_LOGS_EXPORTER=otlp OTEL_SERVICE_NAME=myapp OTEL_RESOURCE_ATTRIBUTES=deployment.environment=staging,service.version=1.0.0 OTEL_LOG_LEVEL=debug ./SampleOpenTelemetry
```
Terminal 3:
===========
// Trigger APIs in an infinite loop
```
while true; do curl localhost:5258; done
```

Access grafana console to see logs in Loki:
===========================================
http://localhost:3000/g4tAIpp4z/logs?orgId=1
![image](https://github.com/bbalakriz/ref-dontet-otel/assets/37283315/6dfaeb7e-18fb-4d35-8de9-63ca1917556e)

Access Prometheus UI to see the standard metrics:
===================================================
http://localhost:9090/graph?g0.expr=process_runtime_dotnet_gc_committed_memory_size&g0.tab=0&g0.stacked=0&g0.show_exemplars=1&g0.range_input=1h
![image](https://github.com/bbalakriz/ref-dontet-otel/assets/37283315/1648c292-581f-4e1a-a5f0-0cdea0810f60)


Access Jaeger UI to see the traces:
===================================
http://localhost:14250/search?end=1692606153523000&limit=20&lookback=5m
![image](https://github.com/bbalakriz/ref-dontet-otel/assets/37283315/a1fd3612-004e-494a-8ebf-07fd94a99870)
