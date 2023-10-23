using Azure.Data.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Socially.Server.DataAccess;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socially.Server.Managers.Utils
{
    public static class QueryExtensions
    {


        public static async Task<int> ExecuteUpdateForAnyDbAsync<TSource>(this IQueryable<TSource> source,
                                                                          Expression<Func<SetPropertyCalls<TSource>, SetPropertyCalls<TSource>>> setPropertyCalls,
                                                                          CancellationToken cancellationToken = default)
        {
            try
            {
                return await source.ExecuteUpdateAsync(setPropertyCalls, cancellationToken);
            }
            catch (InvalidOperationException)
            {
                var items = await source.ToListAsync(cancellationToken);
                List<MethodCallExpression> methodCalls = new();

                ExtractMethodCalls(setPropertyCalls.Body, methodCalls);

                foreach (var methodCall in methodCalls)
                    if (methodCall.Method.Name == "SetProperty")
                    {
                        //// Extract and compile the method into a delegate
                        //var method = typeof(SetPropertyCalls<TSource>).GetMethod("SetProperty").MakeGenericMethod(methodCall.Type.GenericTypeArguments);
                        //var action = (Action<SetPropertyCalls<TSource>, Func<TSource, object>, Func<TSource, object>>)Delegate.CreateDelegate(typeof(Action<SetPropertyCalls<TSource>, Func<TSource, object>, Func<TSource, object>>), method);
                        var getterArg = methodCall.Arguments[0];
                        var setterArg = methodCall.Arguments[1];

                        var compiledGetter = Expression.Lambda<Func<TSource, object>>(Expression.Convert(getterArg, typeof(object)), Expression.Parameter(typeof(TSource), "x")).Compile();
                        var compiledSetter = Expression.Lambda<Func<TSource, object>>(Expression.Convert(setterArg, typeof(object)), Expression.Parameter(typeof(TSource), "x")).Compile();
                        // Extract and compile the method into a delegate
                        //var method = typeof(SetPropertyCalls<TSource>).GetMethod("SetProperty").MakeGenericMethod(methodCall.Type.GenericTypeArguments);
                        //var action = (Action<SetPropertyCalls<TSource>, Func<TSource, object>, Func<TSource, object>>)Delegate.CreateDelegate(typeof(Action<SetPropertyCalls<TSource>, Func<TSource, object>, Func<TSource, object>>), method);


                        // Invoke the delegate on the collection
                        foreach (var item in items)
                        {

                            //var member = (MemberExpression)getterArg.Body;
                            //var propertyInfo = (PropertyInfo)member.Member;

                            // Execute the compiled setter to get the value to set
                            var res = compiledSetter(item);

                            if (getterArg is LambdaExpression pointer
                                    && pointer.Body is MemberExpression member
                                    && member.Member is PropertyInfo info)
                                info.SetValue(item, res);
                            else
                                throw new Exception("Getter is not in the right format in the SetProperty()");


                            // Set the value to the property
                            //propertyInfo.SetValue(item, res);

                            //action(setPropertyCallsInstance, compiledGetter, compiledSetter);

                            //action(setPropertyCallsInstance, null, null);  // Replace null with actual getter and setter functions
                        }
                    }

                await source.GetSourceDbAsync().SaveChangesAsync(cancellationToken);
                return 0;
            }
        }


        
        static DbContext GetSourceDbAsync<TSource>(this IQueryable<TSource> source)
        {

#pragma warning disable EF1001 // Internal EF Core API usage.
            var prov = source.Provider.ForceGetValue("_queryCompiler")
                                          .ForceGetValue("_queryContextFactory")
                                          .ForceGetValue("Dependencies")
                                          .ForceGetValue("CurrentContext") as CurrentDbContext;
#pragma warning restore EF1001 // Internal EF Core API usage.

#pragma warning disable EF1001 // Internal EF Core API usage.
            return prov.Context;
#pragma warning restore EF1001 // Internal EF Core API usage.
        }

        static object ForceGetValue(this object obj, string memberName)
        {
            var type = obj.GetType();
            var propertyInfo = type.GetProperty(memberName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(obj);
            }

            var fieldInfo = type.GetField(memberName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                return fieldInfo.GetValue(obj);
            }

            throw new ArgumentException("No such field or property exists.");
        }


        static void ExtractMethodCalls(Expression expression, List<MethodCallExpression> methodCalls)
        {
            if (expression is MethodCallExpression methodCall)
            {
                methodCalls.Add(methodCall);
                ExtractMethodCalls(methodCall.Object, methodCalls);
                foreach (var argument in methodCall.Arguments)
                {
                    ExtractMethodCalls(argument, methodCalls);
                }
            }
            else if (expression is UnaryExpression unary)
            {
                ExtractMethodCalls(unary.Operand, methodCalls);
            }
            else if (expression is BinaryExpression binary)
            {
                ExtractMethodCalls(binary.Left, methodCalls);
                ExtractMethodCalls(binary.Right, methodCalls);
            }
            else if (expression is LambdaExpression lambda)
            {
                ExtractMethodCalls(lambda.Body, methodCalls);
            }
            // Handle other expression types as needed
        }

    }
}
