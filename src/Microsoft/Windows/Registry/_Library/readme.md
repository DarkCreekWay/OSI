# Operating System Integration Library for the Microsoft Windows Registy

This library is designed with the goal to be easy to integrate into existing codebases with minimum effort.
After integration of the library into a codebase, all registry related code can be easily covered by unit tests.

Having an abstraction for the Microsoft Windows Registry helps you to develop testable Registry related code.
Within unit tests, access to a real registry can be replaced by an InMemory implementation.
This makes testing registry related code simple, reliable, machine independent and repeatable.

---

Copyright (c) 2020 by [DarkCreekWay](https://github.com/DarkCreekWay)