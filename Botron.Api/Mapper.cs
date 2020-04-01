using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace Botron.Api
{
    public class Mapper : IDisposable
    {
        protected static IMapper mapper;

        public static void Configure(Action<Mapper> mappingScope)
        {
            if (mapper != null)
                throw new InvalidOperationException("Mapping can only be configured once");

            using (Mapper mapper = new Mapper())
            {
                mappingScope(mapper);
            }
        }

        List<(Type source, Type dest)> typeMaps = new List<(Type, Type)>();

        protected Mapper()
        {
        }

        public void MapType<TSource, TDest>()
        {
            typeMaps.Add((typeof(TSource), typeof(TDest)));
        }

        public void Dispose()
        {
            mapper = new MapperConfiguration(config =>
            {
                foreach (var item in typeMaps)
                    config.CreateMap(item.source, item.dest);
            }).CreateMapper();
        }
    }

    public abstract class Mapper<TDest> : Mapper
    {
        public static TDest Map<TSource>(TSource value)
        {
            return mapper.Map<TDest>(value);
        }
    }
}
