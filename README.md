# redis_csharp_training

Simple project in order to have a first contact with the C# programming language and redis database

Here we build a simple c# application that accesses Yahoo finance data, populates redis and uses grafana to visualize the data.

## Run

We are using docker compose to configure all the services, so we can run the command below to see everything work.

```bash
docker-compose up
```
And now we can access [grafana](http://localhost:3000/d/cbMdhokVz/quotes?orgId=1&refresh=30s) and see an initial preview of the data.

![](/doc/images/grafana.png "")
