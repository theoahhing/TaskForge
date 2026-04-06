# TaskForge

**TaskForge** is a C# desktop automation engine designed to manage executables, run structured tasks, and execute event-driven workflows.

The project focuses on building a scalable automation system capable of:

* Managing and launching `.exe` files
* Running predefined task sequences
* Executing event-based logic
* Supporting randomized "chaos" operations
* Enabling future remote control via external systems (e.g. Discord bots)

---

## Project Goals

This project is designed as both a **learning tool** and a **portfolio-grade system**, with the following goals:

* Learn and apply C# in real-world scenarios
* Build a clean, scalable architecture (services, models, engines)
* Implement process and file management systems
* Develop an extensible event/task execution engine
* Integrate remote execution via external APIs (future)
* Maintain production-style code structure and practices

---

## Core Concepts

TaskForge is built around the following core ideas:

### 🔹 Tasks

A **task** is a named sequence of actions that can be executed on demand.

### 🔹 Events

An **event** is a single unit of work, such as:

* Launching an executable
* Stopping a process
* Waiting for a duration
* Writing to a file

### 🔹 Services

Services contain the main application logic:

* Process management
* File scanning
* Task execution

### 🔹 Engines

Engines orchestrate behavior across services.

Example:

* Chaos engine (randomized actions)

---

## Project Structure

```text
TaskForge/
├── src/
│   └── TaskForge.App/
│       ├── Models/
│       ├── Services/
│       ├── Engines/
│       ├── Utils/
│       └── Config/
└── tests/
```

---

## Planned Features

### Phase 1 — Core Functionality

* [ ] Scan directories for `.exe` files
* [ ] Launch executables
* [ ] Stop running processes
* [ ] Basic logging system

### Phase 2 — Task System

* [ ] Define tasks using structured data (JSON)
* [ ] Execute sequences of events
* [ ] Add delay and control flow actions

### Phase 3 — Chaos Engine

* [ ] Random event execution
* [ ] Weighted probabilities
* [ ] Safe mode / dry-run mode

### Phase 4 — Desktop UI

* [ ] Basic GUI for managing tasks
* [ ] Process monitoring dashboard
* [ ] Manual event triggering

### Phase 5 — Remote Control

* [ ] Local API server
* [ ] Integration with external bot (e.g. Discord)
* [ ] Remote task execution

---

## Safety Considerations

This project involves executing system-level actions. To ensure safe operation:

* Only trusted executables will be allowed
* Tasks will be predefined and controlled
* Logging will track all actions
* A safe mode will be implemented to simulate execution

---

## Tech Stack

* **Language:** C#
* **Framework:** .NET
* **IDE:** Visual Studio
* **Version Control:** Git + GitHub

---

## Current Status

🚧 Project is in early development — initial structure and core services are being implemented.

---

## Future Improvements

* Plugin system for custom events
* Cross-platform support (via .NET)
* Web dashboard
* Advanced scheduling system

---

## License

This project is licensed under the MIT License.

---

## Author

Developed as part of a personal initiative to build scalable automation systems and strengthen C# development skills.
