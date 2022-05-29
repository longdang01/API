using BasicAuthenticationWebAPI.Authentication;
using BasicAuthenticationWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace BasicAuthenticationWebAPI.Controllers
{
    [BasicAuthentication]
    public class EmployeeController : ApiController
    {
        [HttpGet]
        [Route("api/employees/get")]
        public List<Employee> Get()
        {
            using (EmployeeManagerEntities db = new EmployeeManagerEntities())
                return db.Employees.ToList();
        }
        [HttpGet]
        [Route("api/employees/get-by-id/{id}")]
        public Employee Get(int id)
        {
            using (EmployeeManagerEntities db = new EmployeeManagerEntities())
                return db.Employees.SingleOrDefault(item => item.ID == id);
        }
        [HttpGet]
        [Route("api/employees/get-by-gender")]
        public HttpResponseMessage GetByGender()
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            using (EmployeeManagerEntities db = new EmployeeManagerEntities())
            {
                switch (username.ToLower())
                {
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            db.Employees.Where(e => e.Gender.ToLower() == "male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK,
                            db.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                    default:
                        return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
        }
        [HttpGet]
        [Route("api/employees/search")]
        public List<Employee> Search(string keyword)
        {
            using (EmployeeManagerEntities db = new EmployeeManagerEntities())
            {
                try
                {
                    return db.Employees.Where(e => (e.FirstName + " " + e.LastName).Contains(keyword)).ToList();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
        [HttpPost]
        [Route("api/employees/create")]
        public HttpResponseMessage Post([FromBody] Employee e)
        {
            using (EmployeeManagerEntities db = new EmployeeManagerEntities())
            {
                try
                {
                    db.Employees.Add(e);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);

                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }
        }
        [HttpPut]
        [Route("api/employees/update")]
        public HttpResponseMessage Put([FromBody] Employee e)
        {
            using (EmployeeManagerEntities db = new EmployeeManagerEntities())
            {
                try
                {
                    Employee emp = db.Employees.SingleOrDefault(item => item.ID == e.ID);
                    emp.FirstName = e.FirstName;
                    emp.LastName = e.LastName;
                    emp.Gender = e.Gender;
                    emp.Birthday = e.Birthday;                 
                    emp.Salary = e.Salary;
                    emp.Department_id = e.Department_id;

                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }

        }
        [HttpDelete]
        [Route("api/employees/delete/{id}")]
        public HttpResponseMessage Delete(int id)
        {

            using (EmployeeManagerEntities db = new EmployeeManagerEntities())
            {
                try
                {
                    Employee emp = db.Employees.SingleOrDefault(item => item.ID == id);

                    db.Employees.Remove(emp);
                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                }
            }
        }
    }
}
