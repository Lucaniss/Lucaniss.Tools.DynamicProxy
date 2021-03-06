using System;
using Lucaniss.Tools.DynamicMocks;
using Lucaniss.Tools.DynamicProxy.Tests.Data;
using Lucaniss.Tools.DynamicProxy.Tests.Data.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Lucaniss.Tools.DynamicProxy.Tests
{
    [TestClass]
    public class ProxyBehaviourForClassWithMethodsTest
    {
        [TestMethod]
        public void InvokeProxyMethod_WhenInvokeMethod_ThenInvokeInterceptor()
        {
            // Arrange
            var instance = new TestClassAndMethodWithParameters();
            var interceptoHandlerMock = Mock.Create<IProxyInterceptorHandler<TestClassAndMethodWithParameters>>();

            const String value = "TEST";
            var invocationHandler = new InvocationHandler();

            interceptoHandlerMock
                .SetupMethod(e => e.Handle(Arg.Any<IProxyInvocation<TestClassAndMethodWithParameters>>()))
                .Callback<IProxyInvocation<TestClassAndMethodWithParameters>>(invokation =>
                {
                    invokation.Invoke();

                    invocationHandler.IsInvoked = true;
                    invocationHandler.InvokationArgument = invokation.ArgumentValues[0];
                });

            // Act
            var proxy = Proxy.Create(instance, interceptoHandlerMock.Instance);
            proxy.Echo(value);

            // Assert           
            Assert.IsNotNull(proxy);

            Assert.IsTrue(invocationHandler.IsInvoked);
            Assert.AreEqual(value, invocationHandler.InvokationArgument);
        }
    }
}