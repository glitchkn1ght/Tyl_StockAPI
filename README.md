# Tyl_StockAPI

## Running the solution

- The solution can be tested with either the swagger UI - it should automatically navigate there on starting the API - or with the included postman collection.
- For the tradeProcessor i have included the SQLScripts for the creation of the table and stored procedure. Just create a database named 'TylTrades' and then run the scripts. 
- You might also need to update the connection strings in appsettings.json depending on the security settings on your machine.

## General Design Considerations

- Performance was my main goal in approaching this scenario as i'm aware of how much traffic trading systems tend to deal with.
- This was why trade storage is decoupled from the API itself. The database layer is often where bottlenecks occur and by using service bus
we can create a buffer between the two to prevent slowdown of the requests. A further possibility would be to deploy the TradeProcessor as
it's own microservice and make it scale dynamically based on demand.
- Also we might reconfigure the requests to be processed via GRPC rather than REST as it's generally more performant.
- I considered combining both of the get stock price methods in the controller into one via an additional parameter (e.g. getAllStock prices = true) but this would have increased the complexity around validation logic and would be less intuitive for others trying to integrate with it. Therefore i thought the simplicity of having two different methods was worth having some repeated code. 
- I know having the commonModels project seems unneccessary given it only has two classes in reality it would contain a lot more and be worth the reduction in repeated code.

## Points for improvment

### General
- The model needs more details added to it, such as the timestamps a trade was requested, completed etc.  
- Obeservability. In addition to the existing logs integration with a tool such as splunk should be considered as well as collection of general system metrics.
- Per environment config files. 

### Validation 
- This is perhaps the current weakest area of the design, if working on this project in reality i would talk with business and other engineers to develop a detailed spec.
- In the case of the stock ticker symbols this would be checked against external service such as finhub. This could be cached using Redis or similiar so as to increase performance.
- The validation on the trade model would possibly be better of as a custom filter on the controller, likewise with the ticker symbol validation.

### Testing and Benchmarking
- BenchmarkDotNet is a good library for the latter, it's easy to get up and running and quickly identifies bottlenecks, decided not to include this for time. 
- The unit testing is meant to be illustrative of understanding than completely exhaustive.
