# phi4

In C#, both classes and records are used to define types, but they serve slightly different purposes and have distinct features. Here's a breakdown of their differences:

### Classes

1. **Immutability**: By default, classes in C# are mutable unless you explicitly make them immutable by defining properties as read-only.

2. **Syntax and Usage**:
    - Defined using the `class` keyword.
    - Used to encapsulate data and behavior together.
    - Supports inheritance and allows for complex logic within methods.

3. **Equality**: Equality in classes is reference-based unless you override `Equals()` and `GetHashCode()`. Two class instances are considered equal only if they refer to the same memory location.

4. **Default Behavior**:
    - Allows method overriding through virtual methods or interfaces.
    - Supports object-oriented principles like inheritance, polymorphism, etc.

5. **Example**:
    ```csharp
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public void DisplayInfo()
        {
            Console.WriteLine($"Name: {Name}, Age: {Age}");
        }
    }
    ```

### Records

1. **Immutability**: By default, records are immutable. Properties of a record cannot be changed after an instance is created.

2. **Syntax and Usage**:
    - Defined using the `record` keyword.
    - Used primarily for data-centric types where immutability, value-based equality, and with-expressions (non-destructive mutation) are important.
    - Automatically provides with-expressions to create modified copies of records.

3. **Equality**: Records provide built-in support for value-based equality. Two record instances are considered equal if their values are the same.

4. **Default Behavior**:
    - Provides `Equals()` and `GetHashCode()` methods that compare based on values, not references.
    - Automatically implements members of interfaces like `IEquatable<T>`.
    - Supports with-expressions to create new instances with modified properties.

5. **Example**:
    ```csharp
    public record Person(string Name, int Age);

    // Creating a record instance
    var person1 = new Person("Alice", 30);

    // Using with-expression for non-destructive mutation
    var person2 = person1 with { Age = 31 };
    ```

### Summary

- Use **classes** when you need mutable data and complex behavior, such as methods that change the state of an object.
- Use **records** when you want to represent immutable data structures where equality is based on values rather than references. They are particularly useful in scenarios like functional programming or data transfer objects (DTOs) in domain-driven design.

Records were introduced in C# 9.0 as a way to provide a simpler syntax for defining value-based types, making them a good choice for applications that benefit from immutability and structural equality.
