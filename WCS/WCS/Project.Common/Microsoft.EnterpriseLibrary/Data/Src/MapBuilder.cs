//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    #region EntityAttribute

    /// <summary>
    /// </summary>
    public enum PropertyType
    {
        /// <summary>
        /// 
        /// </summary>
        Null = 0
    ,
        /// <summary>
        /// 
        /// </summary>
        List = 1
    ,
        /// <summary>
        /// 
        /// </summary>
        Detail = 2
    ,
        /// <summary>
        /// 
        /// </summary>
        Insert = 4
    ,
        /// <summary>
        /// 
        /// </summary>
        Update = 8
    ,
        /// <summary>
        /// 
        /// </summary>
        Delete = 16
    ,
        /// <summary>
        /// 
        /// </summary>
        Return = 32
    ,
        /// <summary>
        /// 
        /// </summary>
        List1 = 64
    ,
        /// <summary>
        /// 
        /// </summary>
        Detail1 = 128
    ,
        /// <summary>
        /// 
        /// </summary>
        Insert1 = 256
    ,
        /// <summary>
        /// 
        /// </summary>
        Update1 = 512
    ,
        /// <summary>
        /// 
        /// </summary>
        Delete1 = 1024
    ,
        /// <summary>
        /// 
        /// </summary>
        Update2 = 2048
    ,
        /// <summary>
        /// 
        /// </summary>
        Return1 = 4096
    ,
        /// <summary>
        /// 
        /// </summary>
        Return2 = 8192
    ,
        /// <summary>
        /// 
        /// </summary>
        Return3 = 16384
    ,
        /// <summary>
        /// 
        /// </summary>
        Result = 32768
    ,
        /// <summary>
        /// 
        /// </summary>
        List2 = 65536
    ,
        /// <summary>
        /// 
        /// </summary>
        Result2 = 131072
    ,
        /// <summary>
        /// 
        /// </summary>
        List3 = 262144
    ,
        /// <summary>
        /// 
        /// </summary>
        List4 = 524288
    ,
        /// <summary>
        /// 
        /// </summary>
        List5 = 1048576
    ,
        /// <summary>
        /// 
        /// </summary>
        List6 = 2097152
    ,
        /// <summary>
        /// 
        /// </summary>
        List7 = 4194304
    ,
        /// <summary>
        /// 
        /// </summary>
        List8 = 8388608
    ,
        /// <summary>
        /// 
        /// </summary>
        List9 = 16777216
    ,
        /// <summary>
        /// 
        /// </summary>
        Report1 = 33554432
    ,
        /// <summary>
        /// 
        /// </summary>
        Report2 = 67108864
    ,
        /// <summary>
        /// 
        /// </summary>
        Report3 = 134217728
    ,
        /// <summary>
        /// 
        /// </summary>
        Report4 = 268435456
    ,
        /// <summary>
        /// 
        /// </summary>
        Report5 = 536870912
    ,
        /// <summary>
        /// 
        /// </summary>
        Return4 = 1073741824
    }

    /// <summary>
    /// 
    /// </summary>
    public class EntityAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public PropertyType pType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string columnName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="type"></param>
        public EntityAttribute(string columnName, PropertyType type)
        {
            this.columnName = columnName;
            this.pType = type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public EntityAttribute(PropertyType type)
        {
            this.pType = type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static public bool IsTypeOfProperty(PropertyInfo pi, PropertyType type)
        {
            return pi.GetCustomAttributes(false).Any(
                x => x is EntityAttribute
                && (((EntityAttribute)x).pType & type) == type);
        }

        /// <summary>
        /// 컬럼네임 가져오기
        /// </summary>
        /// <param name="pi"></param>
        /// <returns></returns>
        static public string GetColumnName(PropertyInfo pi)
        {
            EntityAttribute e = pi.GetCustomAttributes(false).First(x => x is EntityAttribute) as EntityAttribute;

            if (e != null)
                return e.columnName;

            return string.Empty;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EntitySet : ArrayList
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public T First<T>(int index) where T : class
        {
            if (base.Count < index + 1)
                return null;

            if (base[index] is T)
                return base[index] as T;

            if (base[index] is List<T>)
            {
                List<T> list = base[index] as List<T>;

                if (list.Count() > 0)
                    return list.First();
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<T> List<T>(int index) where T : class
        {
            if (base.Count < index + 1)
                return null;

            if (base[index] is List<T>)
                return base[index] as List<T>;

            return null;
        }
    }

    #endregion

    /// <summary>
    /// Static entry point for the <see cref="IMapBuilderContext&lt;TResult&gt;"/> interface, which allows to build reflection-based <see cref="IRowMapper&lt;TResult&gt;"/>s.
    /// </summary>
    /// <typeparam name="TResult">The type for which a <see cref="IRowMapper&lt;TResult&gt;"/> should be build.</typeparam>
    /// <seealso cref="IMapBuilderContext&lt;TResult&gt;"/>
    /// <seealso cref="IRowMapper&lt;TResult&gt;"/>
    static public class MapBuilder<TResult>
        where TResult : new()
    {
        /// <summary>
        /// Returns a <see cref="IRowMapper&lt;TResult&gt;"/> that maps all properties for <typeparamref name="TResult"/> based on name.
        /// </summary>
        /// <returns>A new instance of <see cref="IRowMapper&lt;TResult&gt;"/>.</returns>
        static public IRowMapper<TResult> BuildAllProperties()
        {
            return MapAllProperties().Build();
        }

        /// <summary>
        /// Returns a <see cref="IMapBuilderContext&lt;TResult&gt;"/> that can be used to build a <see cref="IRowMapper&lt;TResult&gt;"/>.
        /// The <see cref="IMapBuilderContext&lt;TResult&gt;"/> has a mapping set up for all properties of <typeparamref name="TResult"/> based on name.
        /// </summary>
        /// <seealso cref="IMapBuilderContext&lt;TResult&gt;"/>
        /// <seealso cref="IRowMapper&lt;TResult&gt;"/>
        /// <returns>A new instance of <see cref="IMapBuilderContext&lt;TResult&gt;"/>.</returns>
        static public IMapBuilderContext<TResult> MapAllProperties()
        {
            IMapBuilderContext<TResult> context = new MapBuilderContext();

            var properties =
                from property in typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                where IsAutoMappableProperty(property)
                select property;

            foreach (var property in properties)
            {
                context = context.MapByName(property);
            }
            return context;
        }

        /// <summary>
        /// Returns a <see cref="IMapBuilderContext&lt;TResult&gt;"/> that can be used to build a <see cref="IRowMapper&lt;TResult&gt;"/>.
        /// The <see cref="IMapBuilderContext&lt;TResult&gt;"/> has no mappings to start out with.
        /// </summary>
        /// <seealso cref="IMapBuilderContext&lt;TResult&gt;"/>
        /// <seealso cref="IRowMapper&lt;TResult&gt;"/>
        /// <returns>A new instance of <see cref="IMapBuilderContext&lt;TResult&gt;"/>.</returns>
        static public IMapBuilderContext<TResult> MapNoProperties()
        {
            return new MapBuilderContext();
        }


        private static bool IsAutoMappableProperty(PropertyInfo property)
        {
            return property.CanWrite
              && property.GetIndexParameters().Length == 0
              && !IsCollectionType(property.PropertyType)
            ;
        }

        private static bool IsCollectionType(Type type)
        {
            // string implements IEnumerable, but for our purposes we don't consider it a collection.
            if (type == typeof(string)) return false;

            var interfaces = from inf in type.GetInterfaces()
                             where inf == typeof(IEnumerable) ||
                                 (inf.IsGenericType && inf.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                             select inf;
            return interfaces.Count() != 0;
        }

        #region 추가함수

        /// <summary>
        /// Returns a <see cref="IRowMapper&lt;TResult&gt;"/> that maps all properties for <typeparamref name="TResult"/> based on name.
        /// </summary>
        /// <param name="type">Modifier of Property</param>
        /// <returns>A new instance of <see cref="IRowMapper&lt;TResult&gt;"/>.</returns>
        static public IRowMapper<TResult> BuildAllProperties(PropertyType type)
        {
            return MapAllProperties(type).Build();
        }

        /// <summary>
        /// Returns a <see cref="IMapBuilderContext&lt;TResult&gt;"/> that can be used to build a <see cref="IRowMapper&lt;TResult&gt;"/>.
        /// The <see cref="IMapBuilderContext&lt;TResult&gt;"/> has a mapping set up for all properties of <typeparamref name="TResult"/> based on name.
        /// </summary>
        /// <seealso cref="IMapBuilderContext&lt;TResult&gt;"/>
        /// <seealso cref="IRowMapper&lt;TResult&gt;"/>
        /// <param name="type">Modifier of Property</param>
        /// <returns>A new instance of <see cref="IMapBuilderContext&lt;TResult&gt;"/>.</returns>
        static public IMapBuilderContext<TResult> MapAllProperties(PropertyType type)
        {
            IMapBuilderContext<TResult> context = new MapBuilderContext();

            var properties =
                from property in typeof(TResult).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                where IsAutoMappableProperty(property) && EntityAttribute.IsTypeOfProperty(property, type)
                select property;

            foreach (var property in properties)
            {
                context = context.MapByName(property);
            }
            return context;
        }

        #endregion

        private class MapBuilderContext : IMapBuilderContextTest<TResult>
        {
            private Dictionary<PropertyInfo, PropertyMapping> mappings;

            public MapBuilderContext()
            {
                mappings = new Dictionary<PropertyInfo, PropertyMapping>();
            }

            public IMapBuilderContextMap<TResult, TMember> Map<TMember>(Expression<Func<TResult, TMember>> propertySelector)
            {
                PropertyInfo property = ExtractPropertyInfo<TMember>(propertySelector);

                return new MapBuilderContextMap<TMember>(this, property);
            }

            public IMapBuilderContextMap<TResult, object> Map(PropertyInfo property)
            {
                PropertyInfo normalizedPropertyInfo = NormalizePropertyInfo(property);

                return new MapBuilderContextMap<object>(this, normalizedPropertyInfo);
            }

            public IMapBuilderContext<TResult> MapByName<TMember>(Expression<Func<TResult, TMember>> propertySelector)
            {
                PropertyInfo property = ExtractPropertyInfo<TMember>(propertySelector);

                return MapByName(property);
            }

            public IMapBuilderContext<TResult> MapByName(PropertyInfo property)
            {
                PropertyInfo normalizedPropertyInfo = NormalizePropertyInfo(property);

                return this.Map(normalizedPropertyInfo).ToColumn(normalizedPropertyInfo.Name);
            }

            public IMapBuilderContext<TResult> DoNotMap<TMember>(Expression<Func<TResult, TMember>> propertySelector)
            {
                PropertyInfo property = ExtractPropertyInfo<TMember>(propertySelector);

                return DoNotMap(property);
            }

            public IMapBuilderContext<TResult> DoNotMap(PropertyInfo property)
            {
                PropertyInfo normalizedPropertyInfo = NormalizePropertyInfo(property);

                mappings.Remove(normalizedPropertyInfo);

                return this;
            }

            public IRowMapper<TResult> Build()
            {
                return new ReflectionRowMapper<TResult>(mappings);
            }

            public IEnumerable<PropertyMapping> GetPropertyMappings()
            {
                return this.mappings.Values;
            }

            private static PropertyInfo ExtractPropertyInfo<TMember>(Expression<Func<TResult, TMember>> propertySelector)
            {
                MemberExpression memberExpression = propertySelector.Body as MemberExpression;
                if (memberExpression == null) throw new ArgumentException(Resources.ExceptionArgumentMustBePropertyExpression, "propertySelector");

                PropertyInfo property = memberExpression.Member as PropertyInfo;
                if (property == null) throw new ArgumentException(Resources.ExceptionArgumentMustBePropertyExpression, "propertySelector");

                return NormalizePropertyInfo(property);
            }

            private static PropertyInfo NormalizePropertyInfo(PropertyInfo property)
            {
                if (property == null) throw new ArgumentNullException("property");
                return typeof(TResult).GetProperty(property.Name);
            }

            private class MapBuilderContextMap<TMember> : IMapBuilderContextMap<TResult, TMember>
            {
                PropertyInfo property;
                MapBuilderContext builderContext;

                public MapBuilderContextMap(MapBuilderContext builderContext, PropertyInfo property)
                {
                    this.property = property;
                    this.builderContext = builderContext;
                }

                public IMapBuilderContext<TResult> ToColumn(string columnName)
                {
                    builderContext.mappings[property] = new ColumnNameMapping(property, columnName);

                    return builderContext;
                }

                public IMapBuilderContext<TResult> WithFunc(Func<IDataRecord, TMember> f)
                {
                    Guard.ArgumentNotNull(f, "f");
                    builderContext.mappings[property] = new FuncMapping(property, row => f(row));

                    return builderContext;
                }
            }
        }
    }

    /// <summary>
    /// A fluent interface that can be used to construct a <see cref="IRowMapper&lt;TResult&gt;"/>.
    /// </summary>
    /// <typeparam name="TResult">The type for which a <see cref="IRowMapper&lt;TResult&gt;"/> should be build.</typeparam>
    public interface IMapBuilderContext<TResult> : IFluentInterface
    {
        /// <summary>
        /// Adds a property mapping to the context for <paramref name="property"/> that specifies this property will be mapped to a column with a matching name.
        /// </summary>
        /// <param name="property">The property of <typeparamref name="TResult"/> that should be mapped.</param>
        /// <returns>The fluent interface that can be used further specify mappings.</returns>
        IMapBuilderContext<TResult> MapByName(PropertyInfo property);

        /// <summary>
        /// Adds a property mapping to the context for <paramref name="propertySelector"/> that specifies this property will be mapped to a column with a matching name.
        /// </summary>
        /// <param name="propertySelector">A lambda function that returns the property that should be mapped.</param>
        /// <returns>The fluent interface that can be used further specify mappings.</returns>
        IMapBuilderContext<TResult> MapByName<TMember>(Expression<Func<TResult, TMember>> propertySelector);

        /// <summary>
        /// Adds a property mapping to the context for <paramref name="property"/> that specifies this property will be ignored while mapping.
        /// </summary>
        /// <param name="property">The property of <typeparamref name="TResult"/> that should be mapped.</param>
        /// <returns>The fluent interface that can be used further specify mappings.</returns>
        IMapBuilderContext<TResult> DoNotMap(PropertyInfo property);

        /// <summary>
        /// Adds a property mapping to the context for <paramref name="propertySelector"/> that specifies this property will be ignored while mapping.
        /// </summary>
        /// <param name="propertySelector">A lambda function that returns the property that should be mapped.</param>
        /// <returns>The fluent interface that can be used further specify mappings.</returns>
        IMapBuilderContext<TResult> DoNotMap<TMember>(Expression<Func<TResult, TMember>> propertySelector);

        /// <summary>
        /// Adds a property mapping to the context for <paramref name="propertySelector"/>.
        /// </summary>
        /// <param name="propertySelector">A lambda function that returns the property that should be mapped.</param>
        /// <returns>The fluent interface that can be used to specify how to map this property.</returns>
        IMapBuilderContextMap<TResult, TMember> Map<TMember>(Expression<Func<TResult, TMember>> propertySelector);

        /// <summary>
        /// Adds a property mapping to the context for <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The property of <typeparamref name="TResult"/> that should be mapped.</param>
        /// <returns>The fluent interface that can be used to specify how to map this property.</returns>
        IMapBuilderContextMap<TResult, object> Map(PropertyInfo property);

        /// <summary>
        /// Builds the <see cref="IRowMapper&lt;TResult&gt;"/> that can be used to map data structures to clr types.
        /// </summary>
        /// <returns>An instance of <see cref="IRowMapper&lt;TResult&gt;"/>.</returns>
        IRowMapper<TResult> Build();
    }

    /// <summary>
    /// A fluent interface that can be used to construct a <see cref="IRowMapper&lt;TResult&gt;"/>.
    /// </summary>
    /// <typeparam name="TResult">The type for which a <see cref="IRowMapper&lt;TResult&gt;"/> should be build.</typeparam>
    /// <typeparam name="TMember">The type of the member for which a mapping needs to specified.</typeparam>
    /// <seealso cref="IMapBuilderContext&lt;TResult&gt;"/>
    public interface IMapBuilderContextMap<TResult, TMember> : IFluentInterface
    {
        /// <summary>
        /// Maps the current property to a column with the given name.
        /// </summary>
        /// <param name="columnName">The name of the column the current property should be mapped to.</param>
        /// <returns>The fluent interface that can be used further specify mappings.</returns>
        IMapBuilderContext<TResult> ToColumn(string columnName);

        /// <summary>
        /// Maps the current property to a user specified function.
        /// </summary>
        /// <param name="f">The user specified function that will map the current property.</param>
        /// <returns>The fluent interface that can be used further specify mappings.</returns>
        IMapBuilderContext<TResult> WithFunc(Func<IDataRecord, TMember> f);
    }

    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// </summary>
    /// <seealso cref="IMapBuilderContext{TResult}"/>
    public interface IMapBuilderContextTest<TResult> : IMapBuilderContext<TResult>
    {
        /// <summary>
        /// Returns the list of <see cref="PropertyMapping"/>s that have been accumulated by the context.
        /// </summary>
        /// <returns>The list of <see cref="PropertyMapping"/>.</returns>
        IEnumerable<PropertyMapping> GetPropertyMappings();
    }
}
