# Tyl_StockAPI

## General Design Goals

- Performance and extendability were my main goals in approaching this scenario as i'm aware of how much traffic
trading systems tend to deal with.
- This was why trade storage is decoupled from the API itself. The database layer is often where bottlenecks occur and by using service bus
we can create a buffer between the two to prevent slowdown of the requests. A further possibility would be to deploy the TradeProcessor as
it's own microservice and make it scale dynamically based on demand. 




## Points for improvment
### Validation 
- This is perhaps the current weakest area of the design, if working on this project in reality i would talk with business and other engineers to develop a detailed spec.
- In the case of the stock ticker symbols this would be checked against external service such as finhub. This could be cached using Redis or similiar so as to increase performance.
- The validation on the trade model would possibly be better of as a custom filter on the controller, likewise with the ticker symbol validation.
### Benchmarking
- BenchmarkDotNet is a good library for this, it's easy to get up and running and quickly identifies bottlenecks, decided not to include this for time. 