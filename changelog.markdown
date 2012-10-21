# Changelog
## 1.0.0
* **Breaking change**: dependency resolution has changed internally. The basic functionality should be the same, but there are some new features:
 * Types with value type constructor parameters can be instantiated.
 * `IEnumerable<T>` and `Array` dependencies can be configured directly. For example
    `Configure<IEnumerable<int>>(new List<int> { 1, 2, 5 });`
 * `Lazy<T>` works like `Func<T>` as a dependency.
 * When a type has multiple greediest constructors, the one with the most configured parameters is chosen.

 If this breaks existing tests, please revert to a previous version temporarily and file an issue on [github](https://github.com/machine/machine.fakes/issues).

## 0.6.0
* Fakes created with the Rhino Mocks adapter will now automatically track their property values. (Thanks to [Albert Weinert](https://github.com/DerAlbertCom))
* Updated to Machine.Specifications 0.5.9, NSubstitute 1.4.3 and FakeItEasy 1.7.4582.63.

## 0.5.1
* New icon

## 0.5.0
* Fakes created with the Moq adapter will now automatically track their property values. (Thanks to [Jason Duffett](https://github.com/laazyj))
* Updated to NSubstitute 1.4.2.0.

## 0.4.0
* Machine.Fakes is considered forward compatible to future versions of Machine.Specifications from now on.
* Updated to require at least Machine.Specifications 0.5.7
* Updated to require at least FakeItEasy 1.7.4507.61 and NSubstitute 1.4.0.0.