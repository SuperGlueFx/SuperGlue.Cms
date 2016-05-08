namespace SuperGlue.Cms.Parsing
{
    public class ParsePropertyExpressionPart : IParseModelExpression
    {
        public ModelExpressionParseResult Parse(string part, object model)
        {
            if (model == null)
                return new ModelExpressionParseResult(false, null);

            var property = model.GetType().GetProperty(part);

            if (property == null)
                return new ModelExpressionParseResult(false, null);

            return new ModelExpressionParseResult(true, property.GetValue(model));
        }
    }
}