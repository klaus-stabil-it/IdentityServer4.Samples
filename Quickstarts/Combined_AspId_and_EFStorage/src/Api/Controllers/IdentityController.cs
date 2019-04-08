// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace Api.Controllers
{

   [Route("identity")]
   [Authorize]
   public class IdentityController : ControllerBase
   {
      public IActionResult Get()
      {
         return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
      }
   }


   [Route("garble")]
   [Authorize]
   public class GarbleController : ControllerBase
   {
      public IActionResult Get()
      {
         return Content("Returværdi fra API");
      }
   }

}