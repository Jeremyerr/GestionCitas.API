namespace GestionCitas.API.Dto
{
    public class PaginacionResult<TEntity>
    {
        public List<TEntity> Result { get; set; } = new List<TEntity>();
        public int Total { get; set; }
    }
}