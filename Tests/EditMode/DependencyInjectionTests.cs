using System;
using CodeBase.Runtime.DependencyBox.Framework;
using FluentAssertions;
using NUnit.Framework;
using Tests.TestingTools.Stubs;
using UnityEngine;

namespace CodeBase.Tests.EditMode
{
  public class DependencyInjectionTests
  {
    [Test]
    public void WhenInterfaceBindedToRealization_ThenContainerReturnsInstanceOfSameType()
    {
      // arrange
      var dependencyBox = new DependencyBox();

      // act
      dependencyBox.Bind<IStub>().To<PlainStub>();
      var instance = dependencyBox.Resolve<IStub>();

      // assert
      instance.Should().NotBeNull();
      instance.Should().BeAssignableTo<IStub>();
      instance.Should().BeAssignableTo<PlainStub>();
    }

    [Test]
    public void WhenBind_AndDoSameBind_ThenBindingExceptionIsThrown()
    {
      // arrange
      var dependencyBox = new DependencyBox();

      // act
      Action binding = () =>
      {
        dependencyBox.Bind<IStub>().To<PlainStub>();
        dependencyBox.Bind<IStub>().To<PlainStub>();
      };

      // assert
      binding.Should().Throw<BindingException>();
    }

    [Test]
    public void WhenBindFromInstance_ThenResolveReturnsThatInstance()
    {
      // arrange
      var dependencyBox = new DependencyBox();
      const int value = 5;

      // act
      dependencyBox.Bind<int>().To<int>().FromInstance(value);

      // assert
      dependencyBox.Resolve<int>().Should().Be(value);
    }

    [Test]
    public void WhenBindClassAndItsDependencies_ThenContainerReturnsClassWithProvidedDependencies()
    {
      // arrange
      var dependencyBox = new DependencyBox();
      const int value = 5;

      // act
      dependencyBox.Bind<int>().To<int>().FromInstance(value);
      dependencyBox.Bind<IStub>().To<StubWithDependencies>();
      var stubInstance = dependencyBox.Resolve<IStub>();

      // assert
      stubInstance.GetValue().Should().Be(value);
    }

    [Test]
    public void WhenResolveUnbindedDependency_ThenResolveExceptionIsThrown()
    {
      // arrange
      var dependencyBox = new DependencyBox();

      // act
      Action act = () => dependencyBox.Resolve<IStub>();

      // assert
      act.Should().Throw<ResolveException>();
    }

    [Test]
    public void WhenDependencyBindingHappensAfterClassBindingWhichRequiresThatDependency_ThenExceptionNotThrown()
    {
      // arrange
      var dependencyBox = new DependencyBox();
      const int value = 5;

      // act
      Action act = () =>
      {
        dependencyBox.Bind<IStub>().To<StubWithDependencies>();
        dependencyBox.Bind<int>().To<int>().FromInstance(value);
      };

      // assert
      act.Should().NotThrow();
    }


    [Test]
    public void
      WhenBindedClassWhichHasIntegerDependency_AndAfterBindedIntegerFive_ThenThatClassResolvedObjectValueShouldBeFive()
    {
      // arrange
      var dependencyBox = new DependencyBox();
      const int value = 5;

      // act
      dependencyBox.Bind<IStub>().To<StubWithDependencies>();
      dependencyBox.Bind<int>().To<int>().FromInstance(value);
      var instance = dependencyBox.Resolve<IStub>();

      // assert
      instance.Should().NotBeNull();
      instance.GetValue().Should().Be(value);
    }

    [Test]
    public void
      WhenBindedIntegerFive_AndInstantiateClassWhichRequiresIntegerAsDependency_ThenInstantiatedClassObjectValueReturnFive()
    {
      // arrange
      var dependencyBox = new DependencyBox();
      const int value = 5;

      // act
      dependencyBox.Bind<int>().To<int>().FromInstance(value);
      var stubWithDependencies = dependencyBox.Instantiate<StubWithDependencies>();

      // assert
      stubWithDependencies.GetValue().Should().Be(value);
    }

    [Test]
    public void WhenInstantiateClassWhichDependenciesIsNotBinded_ThenResolveExceptionIsThrown()
    {
      // arrange
      var dependencyBox = new DependencyBox();

      // act
      Action act = () => dependencyBox.Instantiate<StubWithDependencies>();

      // assert
      act.Should().Throw<ResolveException>();
    }

    [Test]
    public void WhenResolveServiceWhichIsNotBinded_ThenResolveExceptionIsThrown()
    {
      // arrange
      var dependencyBox = new DependencyBox();

      // act
      Action act = () => dependencyBox.Resolve<PlainStub>();

      // assert
      act.Should().Throw<ResolveException>();
    }

    [Test]
    public void WhenBind_AndThenUnbind_ThenResolveThrowsResolveException()
    {
      // arrange
      var dependencyBox = new DependencyBox();

      // act
      dependencyBox.Bind<IStub>().To<PlainStub>();
      dependencyBox.Unbind<IStub>();
      Action act = () => dependencyBox.Resolve<IStub>();

      // assert
      act.Should().Throw<ResolveException>();
    }


    [Test]
    public void WhenBindOnlyContract_ThenResolveThrowsResolveException()
    {
      // arrange
      var dependencyBox = new DependencyBox();

      // act
      dependencyBox.Bind<IStub>();
      Action act = () => dependencyBox.Resolve<IStub>();

      // assert
      act.Should().Throw<ResolveException>();
    }

    [Test]
    public void WhenCreateSubContainer_ThenSubContainerShouldNotBeNull()
    {
      // arrange
      var dependencyBox = new DependencyBox();

      // act
      DependencyBox subContainer = dependencyBox.CreateSubContainer();

      // assert
      subContainer.Should().NotBeNull();
    }

    [Test]
    public void WhenResolveParentContainerBindingFromSubContainer_ThenInstanceShouldNotBeNull()
    {
      // arrange
      var dependencyBox = new DependencyBox();

      // act
      dependencyBox.Bind<IStub>().To<PlainStub>();
      DependencyBox subContainer = dependencyBox.CreateSubContainer();
      var instance = subContainer.Resolve<IStub>();

      // assert
      instance.Should().NotBeNull();
    }

    [Test]
    public void WhenInstantiatePrefab_ThenDependenciesInjectedIntoComponent()
    {
      // arrange
      var prefab = new GameObject();
      prefab.AddComponent<StubMonoBehaviourWithDependencies>();

      var stub = new PlainStub();

      var dependencyBox = new DependencyBox();
      dependencyBox.Bind<IStub>().To<PlainStub>().FromInstance(stub);

      // act
      GameObject clone = dependencyBox.InstantiatePrefab(prefab);

      // assert
      clone.Should().NotBeNull();
      var stubMonoBehaviour = clone.GetComponent<StubMonoBehaviourWithDependencies>();
      stubMonoBehaviour.GetStub().Should().Be(stub);
    }

    [Test]
    public void WhenBindMonoBehaviour_ThenGameObjectWithThisMonoBehaviourShouldBeCreated()
    {
      // arrange
      var dependencyBox = new DependencyBox();

      // act
      dependencyBox.Bind<StubMonoBehaviour>();

      // assert
      var instance = dependencyBox.Resolve<StubMonoBehaviour>();
      instance.Should().NotBeNull();
    }

    [Test]
    public void WhenBindMonoBehaviour_ThenAllDependenciesInjected()
    {
      // arrange
      var dependencyBox = new DependencyBox();
      var stub = new PlainStub();

      // act
      dependencyBox.Bind<StubMonoBehaviourWithDependencies>();
      dependencyBox.Bind<IStub>().To<PlainStub>().FromInstance(stub);

      // assert
      var instance = dependencyBox.Resolve<StubMonoBehaviourWithDependencies>();
      instance.GetStub().Should().Be(stub);
    }
  }
}