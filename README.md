# Congestion tax calculator with [Akka.Net](https://getakka.net/)

## Purpose

This application makes use of actor model in order to calculate congestion tax for a registered vehicle. My main ambition is to make myself familiar with the actor model and its capabilities. Since this is my first hands on test with actor model, it only utilises some basic concepts of Akka.net.

---

## How to run

- Open solution and build.
- Press F5 to start the service worker. The console application must be up and running before you start the web application.
- Right click on web project. Under _Debug_, click on _Start new instance_
- Use your choice of client to interact with the API methods below.

---

## API endpoints

### Register vehicle

    POST /api/fordon/register?regnr=abc999&type=999

regnr: _string_; type: _VehicleType_

**VehicleType**

    utryckningsfordon = 1
    bussar med totalvikt av minst 14 ton = 2
    diplomatregistrerade fordon = 3
    motorcyklar = 4
    militära fordon = 5
    other = 999

---

### See all vehicles

    GET /api/fordon/get

---

### Register passage

    POST /api/fordon/registerpass?regnr=abc999&passTime=2020-09-29T12:20:10%2B02:00

regnr: _string_; passTime: _datetime_ (do not forget to URL
encode the date)

---

### Get tax for a given vehicle and day

    GET /api/fordon/GetSummary?regnr=abc999&date=2020-09-29

regnr: _string_; date: _short datetime_

---

## Improvement areas

- [ ] Add external database for saving the data. Right now the data stays in-memory for each vehicle (actor).
- [ ] Import vehicle list from an another source, such as external systems, databases, cache etc. Right now the user has to manually register the vehicle before registering the passages. This is required since the congestion tax calculation is not only based on passage times, but also on vehicle types.
- [ ] Add more endpoints to query data. Right now it is only possible to query for a single day. It would be nice to have an endpoint where the total tax for an entire month is returned. However, this is trivial for the purpose of this exercise.
- [ ] Add memoization for day tax. Right now the day tax is recalculated after each passage and then is updated in an in-memory dictionary. Although this won't create any problems for small amounts of passages, it will affect if the passage history is rather huge. If the current grouped intervals (see GoteborgTrangselskattCalculator.cs) with their respective prices is cached, the performance can be increased.
- [ ] Input validation in Web API layer. Right now there's no input validation in the API layer.
- [ ] Better logging. The console application shows logs for each operation but other assemblies do not have any logging at all.
