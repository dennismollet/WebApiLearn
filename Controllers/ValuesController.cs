using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        static List<Value> People = new List<Value>(){
                new Value { Name = "Aaron", Id = 1 },
                new Value { Name = "Dennis", Id = 2 },
                new Value { Name = "Dave", Id = 3 }
            };

        //Route examples
        //[Route("GetByPersonId")] /*this route becomes api/[controller]/GetByPersonId*/
        //public string GetByPersonId([FromBody] int personId) { }
        //
        //[Route("/someotherapi/[controller]/GetByMemberId")] /* this route becomes someotherapi/[controller]/GetByMemberId.   note the / at the start, you need this to override the route at the controller level */
        //public string GetByMemberID([FromBody int memberId]) { }
        //
        //[Route("IsFirstNumberBigger")] /* this route becomes api/[controller]/IsFirstNumberBigger */
        //public string IsFirstNumberBigger([FromBody] int firstNum, int secondNum) { }
        //

        //action signature level attributes
        //[FromUri] -> http://localhost:8080/api/people?id=1&id=2&id=3
        //[FromRoute] -> /http://localhost:8080/api/people/3
        //[FromBody] 
        //              var content = new StringContent(JsonConvert.SerializeObject(value));
        //              content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        //              var responseRequest = await http.PostAsync("http://localhost:8080/api/people", content);

        //https://www.strathweb.com/2013/04/asp-net-web-api-parameter-binding-part-1-understanding-binding-from-uri/
        // consider a call like this: GET /products?sizes=L,XL,XXL
        // we can create our own ModelBinders to convert uri data into c# objects. This converts a comma-delimmited string into a string array        
        // public class CommaDelimitedCollectionModelBinder : IModelBinder
        // {
        //     public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        //     {
        //         var key = bindingContext.ModelName;
        //         var val = bindingContext.ValueProvider.GetValue(key);
        //         if (val != null)
        //         {
        //             var s = val.AttemptedValue;
        //             if (s != null && s.IndexOf(",", System.StringComparison.Ordinal) > 0)
        //             {
        //                 var stringArray = s.Split(new[] { "," }, StringSplitOptions.None);
        //                 bindingContext.Model = stringArray;
        //             }
        //             else
        //             {
        //                 bindingContext.Model = new[] { s };
        //             }
        //             return true;
        //         }
        //         return false;
        //     }
        // }

        [HttpGet]
        public ActionResult<IEnumerable<Value>> Get()
        {
            return new OkObjectResult(People);
        }

        [HttpGet("{id}")]
        public ActionResult<Value> Get([FromRoute]int id)
        {
            var val = People.FirstOrDefault(v => v.Id == id);

            if (val != null)
            {
                return new OkObjectResult(val);
            }
            else
            {
                return new NotFoundResult();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Value>> Post([FromBody]Value value)
        {
            People.Add(value);
            return new OkObjectResult(value);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Value>> Put([FromRoute]int id, [FromBody]Value value)
        {
            bool created = false;
            var existing = People.FirstOrDefault(v => v.Id == id);

            if (value != null)
            {
                created = true;
                People.Remove(existing);
            }

            People.Add(value);

            if (created)
            {
                return new CreatedResult("/values", value);
            }
            else
            {
                return new OkObjectResult(value);
            }
        }
    }
}
