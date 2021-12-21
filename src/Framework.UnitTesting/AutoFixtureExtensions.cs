using System;
using System.Linq;

using AutoFixture;
using AutoFixture.Kernel;

using NSubstitute;

namespace Framework.UnitTesting
{
    /// <summary>
    /// Extension methods for AutoFixture.
    /// </summary>
    public static class AutoFixtureExtensions
    {
        /// <summary>
        /// Registers the strict mock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="repository">The repository.</param>
        /// <returns>Created and registered in AutoFixture mock instance.</returns>
        public static T RegisterStub<T>(this IFixture fixture
                                              //, MockRepository repository = null
            )
            where T : class
        {
            return fixture.RegisterStub<T>(GetCtorArgTypes<T>()//, repository
                                                 );
        }

        /// <summary>
        /// Registers the strict mock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="ctorArgumentTypes">The ctor argument types.</param>
        /// <param name="repository">The repository.</param>
        /// <returns>Created and registered in AutoFixture mock instance.</returns>
        public static T RegisterStub<T>(
            this IFixture fixture,
            Type[] ctorArgumentTypes//,
            //MockRepository repository = null
            )
            where T : class
        {
            var ctorArgs = CreateCtorArgs(fixture, ctorArgumentTypes);
            return fixture.RegisterStub<T>(ctorArgs//, repository
                                                 );
        }

        /// <summary>
        /// Registers the strict mock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="ctorArgs">The ctor arguments.</param>
        /// <param name="repository">The repository.</param>
        /// <returns>Created and registered in AutoFixture mock instance.</returns>
        public static T RegisterStub<T>(
            this IFixture fixture,
            object[] ctorArgs//,
            //MockRepository repository = null
            )
            where T : class
        {
            var result = Substitute.For<T>(ctorArgs);
            //var result = repository?.StrictMock<T>(ctorArgs) ?? MockRepository.GenerateStrictMock<T>(ctorArgs);
            fixture.Register(() => result);
            return result;
        }

        /// <summary>
        /// Registers the dynamic mock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="repository">The repository.</param>
        /// <returns>Created and registered in AutoFixture mock instance.</returns>
        public static T RegisterDynamicMock<T>(this IFixture fixture//, MockRepository repository = null
            )
            where T : class
        {
            return fixture.RegisterDynamicMock<T>(GetCtorArgTypes<T>()//, repository
                                                  );
        }

        /// <summary>
        /// Registers the dynamic mock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="ctorArgumentTypes">The ctor argument types.</param>
        /// <param name="repository">The repository.</param>
        /// <returns>Created and registered in AutoFixture mock instance.</returns>
        public static T RegisterDynamicMock<T>(
            this IFixture fixture,
            Type[] ctorArgumentTypes//,
            //MockRepository repository = null
                )
            where T : class
        {
            var ctorArgs = CreateCtorArgs(fixture, ctorArgumentTypes);
            return fixture.RegisterDynamicMock<T>(ctorArgs//, repository
                                                  );
        }

        /// <summary>
        /// Registers the dynamic mock.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fixture">The fixture.</param>
        /// <param name="ctorArgs">The ctor arguments.</param>
        /// <param name="repository">The repository.</param>
        /// <returns>Created and registered in AutoFixture mock instance.</returns>
        public static T RegisterDynamicMock<T>(
            this IFixture fixture,
            object[] ctorArgs//,
//            MockRepository repository = null
            )
            where T : class
        {
            var result = Substitute.For<T>(ctorArgs);
            //var result = repository?.DynamicMock<T>(ctorArgs) ?? Substitute.For<T>(ctorArgs);
            fixture.Register(() => result);
            return result;
        }

        private static Type[] GetCtorArgTypes<T>()
            where T : class
        {
            var type = typeof(T);
            var ctors = type.GetConstructors();

            if (ctors.Length > 1)
            {
                throw new InvalidOperationException(
                    $"Found more then one public constructor for type {type.Name}. Specify required constructor arguments types explicitly.");
            }

            var ctor = ctors.SingleOrDefault();
            var ctorArgTypes = ctor?.GetParameters().Select(c => c.ParameterType).ToArray() ?? new Type[0];
            return ctorArgTypes;
        }

        private static object[] CreateCtorArgs(IFixture fixture, Type[] ctorArgumentTypes)
        {
            if (ctorArgumentTypes.Length == 0)
            {
                return new object[0];
            }

            var context = new SpecimenContext(fixture);

            return ctorArgumentTypes
                .Select(t => t.IsInterface ? Substitute.For(new[] { t }, new object[0]) : context.Resolve(t))
                .ToArray();
        }
    }
}
