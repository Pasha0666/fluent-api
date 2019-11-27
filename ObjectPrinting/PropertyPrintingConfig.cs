﻿using System;
using System.Globalization;
using System.Linq.Expressions;

namespace ObjectPrinting
{
    public class PropertyPrintingConfig<TOwner, TPropType> : IPropertyPrintingConfig<TOwner, TPropType>
    {
        private readonly PrintingConfig<TOwner> printingConfig;
        private readonly Expression<Func<TOwner, TPropType>> memberSelector;
        
        
        public PropertyPrintingConfig(PrintingConfig<TOwner> printingConfig, Expression<Func<TOwner, TPropType>> memberSelector = null)
        {
            this.printingConfig = printingConfig;
            this.memberSelector = memberSelector;
        }

        public PrintingConfig<TOwner> Using(Func<TPropType, string> print)
        {
            if (memberSelector == null)
                ((IPrintingConfig) printingConfig).TypePrintingFunctions[typeof(TPropType)] = print;
            else if (memberSelector.Body is MemberExpression memberExpression)
                ((IPrintingConfig) printingConfig).PropertyPrintingFunctions[memberExpression.Member.Name] = print;
            return printingConfig;
        }

        public PrintingConfig<TOwner> Using(CultureInfo culture)
        {
            ((IPrintingConfig) printingConfig).TypeCultures[typeof(TPropType)] = culture;
            return printingConfig;
        }

        PrintingConfig<TOwner> IPropertyPrintingConfig<TOwner, TPropType>.ParentConfig => printingConfig;
        Expression<Func<TOwner, TPropType>>  IPropertyPrintingConfig<TOwner, TPropType>.MemberSelector => memberSelector;
    }

    public interface IPropertyPrintingConfig<TOwner, TPropType>
    {
        PrintingConfig<TOwner> ParentConfig { get; }
        Expression<Func<TOwner, TPropType>> MemberSelector { get; }
    }
}