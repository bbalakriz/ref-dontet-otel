# ref-dontet-otel

Terminal 1:
===========
// Run the complete OpenTelemetry setup
```
cd application/Configs 
docker-compose up
```

Terminal 2:
===========
// Build and run the app
```
cd application/SampleOpenTelemetry
dotnet clean && dotnet build && dotnet run
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


