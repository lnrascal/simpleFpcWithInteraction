Simple FPC with Interaction

**Overview**

Minimal First Person Controller with a modular interaction system.

The project demonstrates a clean approach to handling player movement and extensible object interactions using interfaces.

**Architecture**

**Core ideas:**

Separation of movement and interaction logic

Interface-based design (IInteractable)

Easy extensibility without modifying existing code

Interaction system

Player interacts via a common interface instead of concrete types

Interaction behavior is defined inside each object

IInteractable

Supports multiple interaction types:

InteractionType (Click / Hold / Continuous)

**Features**

First person movement

Mouse look

Raycast-based interaction

Click / Hold / Continuous interactions

Example interactables (e.g. valve rotation)

**Notes**

This is a lightweight foundation meant for extension, not a full production system.
