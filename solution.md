# Sales taxes kata solution


## Introduction
Due to the scale needed, retailers usually rely on multiple distributed systems each one involving many components.

This means that the design and implementation of even only one element has an high level of complexity also to support the versatility required by the business domain.

It is pretty common to use [Domain-Driven Design](https://wikipedia.org/wiki/Domain-driven_design) to analyze an area of the business, a DDD subdomain, maybe with some [EventStorming](https://www.eventstorming.com) sessions, with the goal of getting a better awareness of concepts, processes and rules that belong to it and start talking all the same ubiquitous language.

Multiple subdomains cooperate by means of integration events and web APIs and even within a single subdomain it is possible to find one or more [microservices](https://microservices.io) that, whenever possible, use a [choreography rather than orchestration](https://www.thoughtworks.com/insights/blog/scaling-microservices-event-stream) integration model.

The implementation of a single microservice is often based on the [ports and adapters](https://web.archive.org/web/20060711221010/http://alistair.cockburn.us:80/index.php/Hexagonal_architecture) pattern also known as hexagonal architecture.

Moreover, when different use cases need different levels of scale and flexibility, the [CQRS](https://www.martinfowler.com/bliki/CQRS.html) pattern is used, resulting in different write and read models often needing polyglot persistence, sometimes in combination with [Event Sourcing](https://www.martinfowler.com/eaaDev/EventSourcing.html).

This is what goes under the term [Event-Driven Architecture](https://martinfowler.com/articles/201701-event-driven.html).

The goal is usually taming complexity by decomposing a huge system in multiple simpler parts with only one responsibility.
Unfortunately this brings along a big increase in the overall system complexity and the challenges related to its implementation, deploy (e.g. SDLC, release management, containerization and container orchestrators like [Kubernetes](https://kubernetes.io)), handling ([observability](https://distributed-systems-observability-ebook.humio.com/)), maintenance, evolution.


## Overview
One of the key element for any retailer is the **checkout** which allows customers to complete a purchase and the company to be paid and make profit.

Considering the goal of this kata is to focus on simple design and coding style and skills, the solution should be as simple as possible.

It would be overkill to design and implement a full system with one or more microservices, using DDD, CQRS, ES, a full-fledged hexagonal architecture and code with full instrumentation for observability (logging, application and business metrics monitoring, distributed tracing at least).

The focus should be on the business logic and its implementation with a trade-off on the completeness of functionalities usually exposed by services belonging to other subdomains like catalog, pricing, billing.

Tests should be used to guarantee that main use cases are covered and the software is maintainable and evolvable without introducing misbehaviors or regressions.

As per the build and test phase few or no scripts should be used and it wouldn't make senso to introduce Kubernetes and [Helm](https://helm.sh) at this stage.


## Details
A possible [C#](https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/index) [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) [2.0](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md) implementation of the solution was developed using [Visual Studio](https://visualstudio.microsoft.com).

Code has been tested for the main use cases and behaviors, but no benchmark or test coverage has been used.

The build and test phase supports [Docker](https://www.docker.com)

The high-level [class diagram](diagrams/classdiagram.puml) has been created with [PlantUML](http://plantuml.com):

![PlantUML class diagram](http://www.plantuml.com/plantuml/proxy?cache=no&src=https://raw.githubusercontent.com/luigiberrettini/sales-taxes-kata/master/diagrams/classdiagram.puml)

Further details are provided by the following class diagrams created by Visual studio.

**Namespace SalesTaxesKata.Domain.Sales**

![Sales](/diagrams/sales.png)

**Namespace SalesTaxesKata.Domain.Taxation**

![Taxation](/diagrams/taxation.png)

**Namespace SalesTaxesKata.Domain.Shopping**

![Shopping](/diagrams/shopping.png)

**Namespace SalesTaxesKata.Domain.Payment**

![Payment](/diagrams/payment.png)


## Environment setup

### Development
 1. Install [.NET Core SDK](https://dotnet.microsoft.com/download)
 2. Execute the following commands from the terminal:
    ```shell
    git clone https://github.com/luigiberrettini/sales-taxes-kata
    cd sales-taxes-kata
    ```
 3. Run tests by executing the following commands from the terminal:
    ```shell
    cd src
    dotnet test
    # OR
    ./build/scripts/bootstrap.sh    # Linux shell
    .\build\scripts\bootstrap.ps1   # Windows PowerShell
    ```

### Plain Docker on local Linux
 1. Install [Docker](https://docs.docker.com/install)
 2. Execute the following commands
    ```shell
    git clone https://github.com/luigiberrettini/sales-taxes-kata
    cd sales-taxes-kata
    docker build . -t dnjq

    # Can be run multiple times after editing the code
    docker run -v $(pwd):/app -w /app --rm dnjq ./build/scripts/bootstrap.sh
    
    docker image rm dnjq
    ```

### Docker on Linux virtual machine
 1. Install [VirtualBox and its Extension Pack](http://www.virtualbox.org/wiki/Downloads)
 2. Install [Vagrant](https://www.vagrantup.com/downloads.html)
 3. Download the [Vagrantfile](Vagrantfile) provided with this repo
 4. Execute the command `vagrant up`
 5. Connect via SSH to localhost on port 2200 with username `vagrant` and password `vagrant`
 6. Execute step 2 of the previous section