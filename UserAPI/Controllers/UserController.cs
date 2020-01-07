using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UserDataAccess;

namespace UserAPI.Controllers
{
    public class UserController : ApiController
    {
        public IEnumerable<User> Get()
        {
            using (UserDataEntities userEntities = new UserDataEntities())
            {
                return userEntities.Users.ToList();
            }
        }

        public User Get(int id)
        {
            using (UserDataEntities userEntities = new UserDataEntities())
            {
                var userEntity = userEntities.Users.FirstOrDefault(e => e.UserId == id);
                return userEntity;
            }
        }

        public HttpResponseMessage Post([FromBody] User user)
        {
            using (UserDataEntities userEntities = new UserDataEntities())
            {
                try
                {
                    userEntities.Users.Add(user);
                    userEntities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, user);
                    message.Headers.Location = new Uri(Request.RequestUri + user.UserId.ToString());
                    return message;
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        public HttpResponseMessage Put(int id, [FromBody] User user)
        {
            using (UserDataEntities userEntities = new UserDataEntities())
            {
                try
                {
                    User userEntity = userEntities.Users.FirstOrDefault(e => e.UserId == id);
                    if (userEntity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The user with the id of " + id.ToString() + " was not found.");
                    }
                    else
                    {
                        userEntity.UserPassword = user.UserPassword;
                        userEntity.UserEmail = user.UserEmail;

                        userEntities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, userEntities);
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            using (UserDataEntities userEntities = new UserDataEntities())
            {
                var entity = userEntities.Users.FirstOrDefault(e => e.UserId == id);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "The product with the id of " + id.ToString() + " was not found.");
                }
                else
                {
                    userEntities.Users.Remove(entity);
                    userEntities.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
        }
    }
}
