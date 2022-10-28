using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bokdol2Studio_ProjectP.Services;
using System.Diagnostics;

namespace Bokdol2Studio_ProjectP.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class userController : Controller
    {
        private InformationService _information;

        public userController(InformationService information)
        {
            this._information = information;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult> getUserInfo(string username)
        {
            Stopwatch stopwatch = new Stopwatch(); //객체 선언
            stopwatch.Start(); // 시간측정 시작
            var user = await this._information.getUserInfo_DB(username);
            stopwatch.Stop(); //시간측정 끝

            System.Console.WriteLine("time : " +
                               stopwatch.ElapsedMilliseconds + "ms");

            return Ok(user);
        }




    }
}
