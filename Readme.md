### A short description/information on how I have designed the system.

1. Tried to implement a DDD.
2. Had to move the orchestrator to the application layer, eventhough it had business rules. This was to avoid the depedency on the Microsoft.Extensions.DependencyInjection.Abstractions. 
3. Used Keyed dependencies.
4. Used primary constructors, just to show the c# 12 feature. I know the parameters won't be readonly.
5. The application layer is not clearly separated of the UI concerns. It is tightly coupled to the console and the formatting. Could have added some services to separate the coupling with the console and the output formatting.
6. Enhanced the output to include more detail. (That's what I think 😀)
7. Did not write the complete tests, wrote some tests for the business entities and services.


I might have missed to convey some points, but, let's discuss. 