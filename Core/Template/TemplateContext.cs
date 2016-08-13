using System;
using Core.Collection;

namespace Core.Template
{

    public sealed class TemplateContext
    {

        private readonly HashMap<string, string> _paramterIndex = new HashMap<string, string>();
        private readonly HashMap<string, Type>   _typeIndex     = new HashMap<string, Type>();
        private readonly HashMap<string, object> _instanceIndex = new HashMap<string, object>();

        public TemplateContext AddParameter(string name, string value)
        {
            return this;
        }


    }

}