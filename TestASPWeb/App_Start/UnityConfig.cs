using System;
using System.Configuration;
using DataLibrary;
using DataLibrary.BusinessLogic;
using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.BusinessLogic.Logger;
using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repo;
using DataLibrary.Repository;
using DataLibrary.Repository.RepoInterfaces;
using DataLibrary.Services;
using Unity;
using Unity.AspNet.Mvc;
using Unity.Injection;

namespace TestASPWeb
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            //database
            container.RegisterType<DBContext>(
                    new PerRequestLifetimeManager(),
                    new InjectionConstructor(ConfigurationManager.ConnectionStrings["default"].ConnectionString)
                );
            
            //User
            container.RegisterType<IUserDAL,UserDAL>(new PerRequestLifetimeManager());
            container.RegisterType<IUser,User>(new PerRequestLifetimeManager());
            container.RegisterType<IUserService,UserService>(new PerRequestLifetimeManager());

            //Training
            container.RegisterType<ITrainingDAL, TrainingDAL>(new PerRequestLifetimeManager());
            container.RegisterType<ITraining, Training>(new PerRequestLifetimeManager());
            container.RegisterType<ITrainingService, TrainingService>(new PerRequestLifetimeManager());

            //Department
            container.RegisterType<IDepartmentDAL, DepartmentDAL>(new PerRequestLifetimeManager());
            container.RegisterType<IDepartment, Department>(new PerRequestLifetimeManager());
            container.RegisterType<IDepartmentService, DepartmentService>(new PerRequestLifetimeManager());


            //Prerequisite
            container.RegisterType<IPrerequisite, Prerequisite>(new PerRequestLifetimeManager());
            container.RegisterType<IPrerequisiteDAL, PrerequisiteDAL>(new PerRequestLifetimeManager());
            container.RegisterType<IPrerequisiteService, PrerequisiteService>(new PerRequestLifetimeManager());

            //Enrollment
            container.RegisterType<IEnrollment, Enrollment>(new PerRequestLifetimeManager());
            container.RegisterType<IEnrollmentDAL, EnrollmentDAL>(new PerRequestLifetimeManager());
            container.RegisterType<IEnrollmentService, EnrollmentService>(new PerRequestLifetimeManager());

            //Logger
            container.RegisterType<ILogger, Logger>(new PerRequestLifetimeManager());

            //role
            container.RegisterType<IRole, Role>(new PerRequestLifetimeManager());

        }
    }
}