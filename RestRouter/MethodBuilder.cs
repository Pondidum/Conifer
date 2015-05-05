using System;
using System.Linq;
using System.Linq.Expressions;

namespace RestRouter
{
	public class MethodBuilder
	{
		private static MethodModel GetMethodInfoInternal(dynamic expression)
		{
			var method = expression.Body as MethodCallExpression;
			if (method == null) throw new ArgumentException("Expression is incorrect!");

			var parameters = method.Method.GetParameters();
			var values = method.Arguments.Select(a => Expression.Lambda(a).Compile().DynamicInvoke()).ToList();

			return new MethodModel
			{
				Method = method.Method,
				Parameters = parameters
					.Select((p, i) => new
					{
						Name = p.Name,
						Value = values[i]
					})
					.ToDictionary(p => p.Name, p => p.Value)
			};
		}

		public static MethodModel GetMethodInfo<T>(Expression<Action<T>> expression)
		{
			var info = GetMethodInfoInternal(expression);
			info.Class = typeof(T);

			return info;
		} 
	}
}