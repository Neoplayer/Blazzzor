using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blazzzor.Server.Controllers
{
    #region snippet_Inherit
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    //[Route("[controller]")]
    public class PetsController : ControllerBase
    #endregion
    {
        private static readonly List<Pet> _petsInMemoryStore = new List<Pet>();

        public PetsController()
        {
            if (_petsInMemoryStore.Count == 0)
            {
                _petsInMemoryStore.Add(
                    new Pet
                    {
                        Breed = "Collie",
                        Id = 1,
                        Name = "Fido"
                    });
            }
        }

        [HttpGet]
        [Route("[Controller]/GetAll")]
        public ActionResult<List<Pet>> GetAll() => _petsInMemoryStore;

        [HttpGet("{id}")]

        [Route("[Controller]/GetById/{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Pet> GetById(int id)
        {
            var pet = _petsInMemoryStore.FirstOrDefault(p => p.Id == id);

            #region snippet_ProblemDetailsStatusCode
            if (pet == null)
            {
                return NotFound();
            }
            #endregion

            return pet;
        }

        #region snippet_400And201
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("[Controller]/Create")]
        public ActionResult<Pet> Create(Pet pet)
        {
            pet.Id = _petsInMemoryStore.Any() ?
                _petsInMemoryStore.Max(p => p.Id) + 1 : 1;
            _petsInMemoryStore.Add(pet);

            return CreatedAtAction(nameof(GetById), new { id = pet.Id }, pet);
        }
        #endregion
    }

    public class Pet
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public String Breed { get; set; }
    }
}