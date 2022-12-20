using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorSozluk.Common.Infrastructure.Results
{
	public class ValidationResponseModel
	{
		public IEnumerable<string> Errors { get; set; }

		public ValidationResponseModel()
		{

		}
		public ValidationResponseModel(IEnumerable<string> errors)
		{
			Errors = errors;
		}
		public ValidationResponseModel(string meesage) : this(new List<string>() { meesage })
		{

		}
		[JsonIgnore]
		public string FlattenErrors => Errors != null
			? string.Join(Environment.NewLine, Errors)
			: string.Empty;
	}
}
