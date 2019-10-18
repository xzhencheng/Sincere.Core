using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Sincere.Core.WebAPI.ServiceExtension
{
    /// <summary>
    /// IServiceCollection扩展
    /// </summary>
    /// <summary>
    /// IServiceCollection扩展
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// 用DI批量注入接口程序集中对应的实现类。
        /// <para>
        /// 需要注意的是，这里有如下约定：
        /// IUserService --> UserService, IUserRepository --> UserRepository.
        /// </para>
        /// </summary>
        /// <param name="service"></param>
        /// <param name="interfaceAssemblyName">接口程序集的名称（不包含文件扩展名）</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAssembly(this IServiceCollection service, string interfaceAssemblyName, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (string.IsNullOrEmpty(interfaceAssemblyName))
                throw new ArgumentNullException(nameof(interfaceAssemblyName));

            var assembly = RuntimeHelper.GetAssembly(interfaceAssemblyName);
            if (assembly == null)
            {
                throw new DllNotFoundException($"the dll \"{interfaceAssemblyName}\" not be found");
            }

            //过滤掉非接口及泛型接口
            var types = assembly.GetTypes().Where(t => t.GetTypeInfo().IsInterface && !t.GetTypeInfo().IsGenericType);

            foreach (var type in types)
            {
                var implementTypeName = type.Name.Substring(1);
                var implementType = RuntimeHelper.GetImplementType(implementTypeName, type);
                if (implementType != null)
                {
                    switch (serviceLifetime)
                    {
                        //根据条件，选择注册依赖的方法
                        case ServiceLifetime.Scoped:
                            //将获取到的接口和类注册进去
                            service.AddScoped(type, implementType);
                            break;
                        case ServiceLifetime.Singleton:
                            service.AddSingleton(type, implementType);
                            break;
                        case ServiceLifetime.Transient:
                            service.AddTransient(type, implementType);
                            break;
                    }

                }
            }
            return service;
        }

       
    }
}
