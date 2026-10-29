var builder = WebApplication.CreateBuilder(args);

// Documentación OpenAPI (solo se expone en desarrollo, ver más abajo)
builder.Services.AddOpenApi();

// CORS: el frontend (Firebase) y la API (Render) viven en dominios distintos.
// Sin esta política el navegador bloquea las respuestas aunque el servidor
// responda bien. AllowAnyOrigin es aceptable para un proyecto académico con
// datos públicos; en producción real se limitaría al dominio del frontend.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

// =====================================================================
// "Base de datos" en memoria.
// Limitación a explicar en la exposición: los datos viven en la memoria
// del proceso. Cuando Render reinicia o "duerme" el servicio, los datos
// vuelven al estado inicial (los pedidos nuevos se pierden).
// =====================================================================
var categorias = new List<Categoria>
{
    new Categoria { Id = 1,  Nombre = "Polos",          Descripcion = "Polos básicos, oversize y deportivos." },
    new Categoria { Id = 2,  Nombre = "Camisas",        Descripcion = "Camisas casuales y formales." },
    new Categoria { Id = 3,  Nombre = "Pantalones",     Descripcion = "Jeans, cargo y pantalones de vestir." },
    new Categoria { Id = 4,  Nombre = "Vestidos",       Descripcion = "Vestidos casuales y elegantes." },
    new Categoria { Id = 5,  Nombre = "Casacas",        Descripcion = "Casacas de denim, impermeables y de cuero sintético." },
    new Categoria { Id = 6,  Nombre = "Calzado",        Descripcion = "Zapatillas y botines." },
    new Categoria { Id = 7,  Nombre = "Sudaderas",      Descripcion = "Hoodies y sudaderas." },
    new Categoria { Id = 8,  Nombre = "Faldas",         Descripcion = "Faldas plisadas y de denim." },
    new Categoria { Id = 9,  Nombre = "Accesorios",     Descripcion = "Gorras, bufandas, cinturones y carteras." },
    new Categoria { Id = 10, Nombre = "Ropa deportiva", Descripcion = "Shorts, leggings, polos y buzos para entrenar." },
};

var productos = new List<Producto>
{
    new Producto { Id = 1, Codigo = "POL001", Nombre = "Polo Básico Cuello Redondo", CategoriaId = 1, Genero = "Hombre", Talla = "M", Color = "Negro", Precio = 39.90m, Stock = 25, Marca = "Urban Style", Material = "Algodón", Temporada = "Todo el año", Descuento = 0, Imagen = "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?auto=format&fit=crop&w=800&q=80", Descripcion = "Polo básico de algodón, ideal para uso diario." },
    new Producto { Id = 2, Codigo = "POL002", Nombre = "Polo Básico Blanco", CategoriaId = 1, Genero = "Hombre", Talla = "L", Color = "Blanco", Precio = 39.90m, Stock = 18, Marca = "Urban Style", Material = "Algodón", Temporada = "Todo el año", Descuento = 10, Imagen = "https://images.unsplash.com/photo-1518611012118-696072aa579a?auto=format&fit=crop&w=800&q=80", Descripcion = "Polo blanco de algodón con diseño clásico." },
    new Producto { Id = 3, Codigo = "POL003", Nombre = "Polo Oversize Negro", CategoriaId = 1, Genero = "Mujer", Talla = "S", Color = "Negro", Precio = 49.90m, Stock = 12, Marca = "Street Wear", Material = "Algodón Premium", Temporada = "Verano", Descuento = 15, Imagen = "https://images.unsplash.com/photo-1529139574466-a303027c1d8b?auto=format&fit=crop&w=800&q=80", Descripcion = "Polo oversize de estilo urbano y moderno." },
    new Producto { Id = 4, Codigo = "POL004", Nombre = "Polo Deportivo Dry Fit", CategoriaId = 1, Genero = "Hombre", Talla = "XL", Color = "Azul", Precio = 59.90m, Stock = 20, Marca = "Sport Max", Material = "Poliéster", Temporada = "Todo el año", Descuento = 5, Imagen = "https://images.unsplash.com/photo-1579952363873-27f3bade9f55?auto=format&fit=crop&w=800&q=80", Descripcion = "Polo deportivo ligero con tecnología de secado rápido." },
    new Producto { Id = 5, Codigo = "CAM001", Nombre = "Camisa Casual Manga Larga", CategoriaId = 2, Genero = "Hombre", Talla = "M", Color = "Celeste", Precio = 79.90m, Stock = 14, Marca = "Classic Wear", Material = "Algodón", Temporada = "Primavera", Descuento = 0, Imagen = "https://images.unsplash.com/photo-1602810318383-e386cc2a3ccf?auto=format&fit=crop&w=800&q=80", Descripcion = "Camisa casual de manga larga para ocasiones informales." },
    new Producto { Id = 6, Codigo = "CAM002", Nombre = "Camisa Formal Blanca", CategoriaId = 2, Genero = "Hombre", Talla = "L", Color = "Blanco", Precio = 99.90m, Stock = 10, Marca = "Executive", Material = "Algodón", Temporada = "Todo el año", Descuento = 10, Imagen = "https://images.unsplash.com/photo-1598033129183-c4f50c736f10?auto=format&fit=crop&w=800&q=80", Descripcion = "Camisa formal blanca ideal para oficina y eventos." },
    new Producto { Id = 7, Codigo = "CAM003", Nombre = "Camisa de Lino", CategoriaId = 2, Genero = "Mujer", Talla = "M", Color = "Beige", Precio = 109.90m, Stock = 8, Marca = "Natural Fashion", Material = "Lino", Temporada = "Verano", Descuento = 20, Imagen = "https://images.unsplash.com/photo-1605763240000-7e93b172d754?auto=format&fit=crop&w=800&q=80", Descripcion = "Camisa ligera de lino con estilo elegante." },
    new Producto { Id = 8, Codigo = "PAN001", Nombre = "Jean Clásico Azul", CategoriaId = 3, Genero = "Hombre", Talla = "32", Color = "Azul", Precio = 119.90m, Stock = 16, Marca = "Denim Pro", Material = "Denim", Temporada = "Todo el año", Descuento = 0, Imagen = "https://images.unsplash.com/photo-1542272604-787c3835535d?auto=format&fit=crop&w=800&q=80", Descripcion = "Jean clásico de corte recto y color azul." },
    new Producto { Id = 9, Codigo = "PAN002", Nombre = "Jean Skinny Negro", CategoriaId = 3, Genero = "Mujer", Talla = "28", Color = "Negro", Precio = 129.90m, Stock = 11, Marca = "Denim Pro", Material = "Denim Stretch", Temporada = "Todo el año", Descuento = 15, Imagen = "https://images.unsplash.com/photo-1541099649105-f69ad21f3246?auto=format&fit=crop&w=800&q=80", Descripcion = "Jean skinny con material stretch para mayor comodidad." },
    new Producto { Id = 10, Codigo = "PAN003", Nombre = "Pantalón Cargo Verde", CategoriaId = 3, Genero = "Hombre", Talla = "34", Color = "Verde Militar", Precio = 139.90m, Stock = 7, Marca = "Street Wear", Material = "Algodón", Temporada = "Otoño", Descuento = 10, Imagen = "https://images.unsplash.com/photo-1517445312882-bc9910d016b7?auto=format&fit=crop&w=800&q=80", Descripcion = "Pantalón cargo con múltiples bolsillos." },
    new Producto { Id = 11, Codigo = "PAN004", Nombre = "Pantalón de Vestir", CategoriaId = 3, Genero = "Hombre", Talla = "32", Color = "Gris", Precio = 149.90m, Stock = 9, Marca = "Executive", Material = "Poliéster", Temporada = "Todo el año", Descuento = 5, Imagen = "https://images.unsplash.com/photo-1473966968600-fa801b869a1a?auto=format&fit=crop&w=800&q=80", Descripcion = "Pantalón de vestir para oficina y eventos formales." },
    new Producto { Id = 12, Codigo = "VES001", Nombre = "Vestido Floral", CategoriaId = 4, Genero = "Mujer", Talla = "S", Color = "Rojo", Precio = 129.90m, Stock = 13, Marca = "Bella Moda", Material = "Viscosa", Temporada = "Primavera", Descuento = 10, Imagen = "https://images.unsplash.com/photo-1496747611176-843222e1e57c?auto=format&fit=crop&w=800&q=80", Descripcion = "Vestido floral ligero y cómodo." },
    new Producto { Id = 13, Codigo = "VES002", Nombre = "Vestido Elegante Negro", CategoriaId = 4, Genero = "Mujer", Talla = "M", Color = "Negro", Precio = 189.90m, Stock = 6, Marca = "Bella Moda", Material = "Poliéster", Temporada = "Todo el año", Descuento = 15, Imagen = "https://images.unsplash.com/photo-1566174053879-31528523f8ae?auto=format&fit=crop&w=800&q=80", Descripcion = "Vestido negro elegante para ocasiones especiales." },
    new Producto { Id = 14, Codigo = "VES003", Nombre = "Vestido Casual Corto", CategoriaId = 4, Genero = "Mujer", Talla = "L", Color = "Azul", Precio = 99.90m, Stock = 15, Marca = "Urban Style", Material = "Algodón", Temporada = "Verano", Descuento = 5, Imagen = "https://images.unsplash.com/photo-1515372039744-b8f02a3ae446?auto=format&fit=crop&w=800&q=80", Descripcion = "Vestido casual corto para uso diario." },
    new Producto { Id = 15, Codigo = "CHA001", Nombre = "Casaca Denim", CategoriaId = 5, Genero = "Hombre", Talla = "L", Color = "Azul", Precio = 179.90m, Stock = 8, Marca = "Denim Pro", Material = "Denim", Temporada = "Otoño", Descuento = 10, Imagen = "https://images.unsplash.com/photo-1578681994506-b8f463449011?auto=format&fit=crop&w=800&q=80", Descripcion = "Casaca denim clásica para días frescos." },
    new Producto { Id = 16, Codigo = "CHA002", Nombre = "Casaca Impermeable", CategoriaId = 5, Genero = "Hombre", Talla = "M", Color = "Negro", Precio = 219.90m, Stock = 5, Marca = "Adventure", Material = "Nylon", Temporada = "Invierno", Descuento = 20, Imagen = "https://images.unsplash.com/photo-1556821840-3a63f95609a7?auto=format&fit=crop&w=800&q=80", Descripcion = "Casaca impermeable para actividades al aire libre." },
    new Producto { Id = 17, Codigo = "CHA003", Nombre = "Casaca de Cuero", CategoriaId = 5, Genero = "Mujer", Talla = "M", Color = "Marrón", Precio = 299.90m, Stock = 4, Marca = "Premium Leather", Material = "Cuero sintético", Temporada = "Invierno", Descuento = 10, Imagen = "https://images.unsplash.com/photo-1551028719-00167b16eac5?auto=format&fit=crop&w=800&q=80", Descripcion = "Casaca de estilo clásico con acabado tipo cuero." },
    new Producto { Id = 18, Codigo = "ZAP001", Nombre = "Zapatillas Urbanas", CategoriaId = 6, Genero = "Hombre", Talla = "42", Color = "Blanco", Precio = 159.90m, Stock = 10, Marca = "Urban Shoes", Material = "Sintético", Temporada = "Todo el año", Descuento = 5, Imagen = "https://images.unsplash.com/photo-1542291026-7eec264c27ff?auto=format&fit=crop&w=800&q=80", Descripcion = "Zapatillas urbanas cómodas para uso diario." },
    new Producto { Id = 19, Codigo = "ZAP002", Nombre = "Zapatillas Deportivas", CategoriaId = 6, Genero = "Mujer", Talla = "38", Color = "Rosado", Precio = 189.90m, Stock = 7, Marca = "Sport Max", Material = "Malla", Temporada = "Todo el año", Descuento = 15, Imagen = "https://images.unsplash.com/photo-1552346154-21d32810aba3?auto=format&fit=crop&w=800&q=80", Descripcion = "Zapatillas deportivas ligeras para entrenamiento." },
    new Producto { Id = 20, Codigo = "ZAP003", Nombre = "Botines de Cuero", CategoriaId = 6, Genero = "Mujer", Talla = "37", Color = "Negro", Precio = 249.90m, Stock = 5, Marca = "Premium Leather", Material = "Cuero sintético", Temporada = "Invierno", Descuento = 20, Imagen = "https://images.unsplash.com/photo-1543163521-1bf539c55dd2?auto=format&fit=crop&w=800&q=80", Descripcion = "Botines elegantes para combinar con diferentes estilos." },
    new Producto { Id = 21, Codigo = "SUD001", Nombre = "Hoodie Básico", CategoriaId = 7, Genero = "Hombre", Talla = "L", Color = "Gris", Precio = 119.90m, Stock = 17, Marca = "Street Wear", Material = "Algodón", Temporada = "Invierno", Descuento = 10, Imagen = "https://images.unsplash.com/photo-1556821840-3a63f95609a7?auto=format&fit=crop&w=800&q=80", Descripcion = "Hoodie cómodo con capucha y bolsillo frontal." },
    new Producto { Id = 22, Codigo = "SUD002", Nombre = "Hoodie Oversize", CategoriaId = 7, Genero = "Mujer", Talla = "M", Color = "Lila", Precio = 129.90m, Stock = 12, Marca = "Urban Style", Material = "Algodón", Temporada = "Invierno", Descuento = 15, Imagen = "https://images.unsplash.com/photo-1620799140408-edc6dcb6d633?auto=format&fit=crop&w=800&q=80", Descripcion = "Sudadera oversize de estilo juvenil." },
    new Producto { Id = 23, Codigo = "FAL001", Nombre = "Falda Plisada", CategoriaId = 8, Genero = "Mujer", Talla = "S", Color = "Negro", Precio = 89.90m, Stock = 10, Marca = "Bella Moda", Material = "Poliéster", Temporada = "Primavera", Descuento = 10, Imagen = "https://images.unsplash.com/photo-1496747611176-843222e1e57c?auto=format&fit=crop&w=800&q=80", Descripcion = "Falda plisada de estilo elegante y juvenil." },
    new Producto { Id = 24, Codigo = "FAL002", Nombre = "Falda Denim", CategoriaId = 8, Genero = "Mujer", Talla = "M", Color = "Azul", Precio = 79.90m, Stock = 14, Marca = "Denim Pro", Material = "Denim", Temporada = "Verano", Descuento = 5, Imagen = "https://images.unsplash.com/photo-1582142306909-195724d33ffc?auto=format&fit=crop&w=800&q=80", Descripcion = "Falda denim corta para un look casual." },
    new Producto { Id = 25, Codigo = "ACC001", Nombre = "Gorra Urbana", CategoriaId = 9, Genero = "Unisex", Talla = "Única", Color = "Negro", Precio = 39.90m, Stock = 30, Marca = "Urban Style", Material = "Algodón", Temporada = "Todo el año", Descuento = 0, Imagen = "https://images.unsplash.com/photo-1521369909029-2afed882baee?auto=format&fit=crop&w=800&q=80", Descripcion = "Gorra urbana ajustable." },
    new Producto { Id = 26, Codigo = "ACC002", Nombre = "Bufanda de Invierno", CategoriaId = 9, Genero = "Unisex", Talla = "Única", Color = "Gris", Precio = 49.90m, Stock = 22, Marca = "Winter Fashion", Material = "Lana", Temporada = "Invierno", Descuento = 10, Imagen = "https://images.unsplash.com/photo-1520903920243-00d872a2d1c9?auto=format&fit=crop&w=800&q=80", Descripcion = "Bufanda abrigadora para temporada de frío." },
    new Producto { Id = 27, Codigo = "ACC003", Nombre = "Cinturón Clásico", CategoriaId = 9, Genero = "Hombre", Talla = "M", Color = "Marrón", Precio = 59.90m, Stock = 18, Marca = "Classic Wear", Material = "Cuero sintético", Temporada = "Todo el año", Descuento = 5, Imagen = "https://images.unsplash.com/photo-1624222247344-550fb60583dc?auto=format&fit=crop&w=800&q=80", Descripcion = "Cinturón clásico para vestir o uso casual." },
    new Producto { Id = 28, Codigo = "ACC004", Nombre = "Cartera Casual", CategoriaId = 9, Genero = "Mujer", Talla = "Única", Color = "Beige", Precio = 89.90m, Stock = 9, Marca = "Bella Moda", Material = "Cuero sintético", Temporada = "Todo el año", Descuento = 15, Imagen = "https://images.unsplash.com/photo-1584917865442-de89df76afd3?auto=format&fit=crop&w=800&q=80", Descripcion = "Cartera casual con varios compartimentos." },
    new Producto { Id = 29, Codigo = "DEP001", Nombre = "Short Deportivo", CategoriaId = 10, Genero = "Hombre", Talla = "M", Color = "Negro", Precio = 69.90m, Stock = 20, Marca = "Sport Max", Material = "Poliéster", Temporada = "Verano", Descuento = 10, Imagen = "https://images.unsplash.com/photo-1591195853828-11db59a44f6b?auto=format&fit=crop&w=800&q=80", Descripcion = "Short deportivo ligero para entrenamiento." },
    new Producto { Id = 30, Codigo = "DEP002", Nombre = "Leggings Deportivos", CategoriaId = 10, Genero = "Mujer", Talla = "S", Color = "Negro", Precio = 79.90m, Stock = 16, Marca = "Sport Max", Material = "Poliéster Stretch", Temporada = "Todo el año", Descuento = 15, Imagen = "https://images.unsplash.com/photo-1579952363873-27f3bade9f55?auto=format&fit=crop&w=800&q=80", Descripcion = "Leggings deportivos de alta elasticidad." },
    new Producto { Id = 31, Codigo = "DEP003", Nombre = "Polo Deportivo Mujer", CategoriaId = 10, Genero = "Mujer", Talla = "M", Color = "Azul", Precio = 64.90m, Stock = 19, Marca = "Sport Max", Material = "Dry Fit", Temporada = "Todo el año", Descuento = 5, Imagen = "https://images.unsplash.com/photo-1518611012118-696072aa579a?auto=format&fit=crop&w=800&q=80", Descripcion = "Polo deportivo transpirable para entrenamiento." },
    new Producto { Id = 32, Codigo = "DEP004", Nombre = "Buzo Deportivo", CategoriaId = 10, Genero = "Unisex", Talla = "L", Color = "Gris", Precio = 149.90m, Stock = 8, Marca = "Sport Max", Material = "Algodón", Temporada = "Invierno", Descuento = 20, Imagen = "https://images.unsplash.com/photo-1556906781-9a412961c28c?auto=format&fit=crop&w=800&q=80", Descripcion = "Buzo deportivo completo para entrenamiento y uso casual." },
};

var clientes = new List<Cliente>
{
    new Cliente { Id = 1, Nombre = "Renzo Quispe",  Correo = "renzo.quispe@example.com",  Telefono = "987654321" },
    new Cliente { Id = 2, Nombre = "Camila Rojas",  Correo = "camila.rojas@example.com",  Telefono = "912345678" },
    new Cliente { Id = 3, Nombre = "Luis Paredes",  Correo = "luis.paredes@example.com",  Telefono = "998877665" },
};

// Promociones: campañas comerciales por categoría. "Descuento" es el máximo
// porcentaje anunciado; el descuento real de cada producto vive en Producto.
var promociones = new List<Promocion>
{
    new Promocion { Id = 1, Titulo = "Hasta 15% en polos",          Descripcion = "Renueva tus básicos con descuentos en polos seleccionados.", Descuento = 15, CategoriaId = 1,  VigenteHasta = "2026-12-31" },
    new Promocion { Id = 2, Titulo = "Hasta 20% en casacas",        Descripcion = "Prepárate para el frío con casacas en oferta.",              Descuento = 20, CategoriaId = 5,  VigenteHasta = "2026-12-31" },
    new Promocion { Id = 3, Titulo = "Hasta 20% en calzado",        Descripcion = "Zapatillas y botines con precio especial.",                  Descuento = 20, CategoriaId = 6,  VigenteHasta = "2026-12-31" },
    new Promocion { Id = 4, Titulo = "Hasta 20% en ropa deportiva", Descripcion = "Todo para entrenar, con descuentos por tiempo limitado.",     Descuento = 20, CategoriaId = 10, VigenteHasta = "2026-12-31" },
};

var pedidos = new List<Pedido>
{
    new Pedido
    {
        Id = 1, ClienteId = 1, Fecha = "2026-10-02 10:15", Estado = "Entregado", Total = 199.70m,
        Items = new List<ItemPedido>
        {
            new ItemPedido { ProductoId = 1, Nombre = "Polo Básico Cuello Redondo", Cantidad = 2, PrecioUnitario = 39.90m },
            new ItemPedido { ProductoId = 8, Nombre = "Jean Clásico Azul",          Cantidad = 1, PrecioUnitario = 119.90m },
        }
    },
    new Pedido
    {
        Id = 2, ClienteId = 2, Fecha = "2026-10-03 16:40", Estado = "Pendiente", Total = 206.33m,
        Items = new List<ItemPedido>
        {
            new ItemPedido { ProductoId = 13, Nombre = "Vestido Elegante Negro", Cantidad = 1, PrecioUnitario = 161.42m },
            new ItemPedido { ProductoId = 26, Nombre = "Bufanda de Invierno",    Cantidad = 1, PrecioUnitario = 44.91m },
        }
    },
};

var siguienteIdCategoria = categorias.Count + 1;
var siguienteIdProducto = productos.Count + 1;
var siguienteIdCliente = clientes.Count + 1;
var siguienteIdPromocion = promociones.Count + 1;
var siguienteIdPedido = pedidos.Count + 1;

// Las peticiones HTTP se atienden en hilos distintos; una compra modifica el
// stock, así que esa sección se protege con un lock para que dos pedidos
// simultáneos no vendan la misma unidad dos veces.
var candado = new object();

// ---------------------------------------------------------------------
// Rutas informativas
// ---------------------------------------------------------------------
app.MapGet("/", () => "API Tienda de ropa funcionando");

app.MapGet("/api", () => Results.Ok(new
{
    recursos = new[] { "productos", "categorias", "clientes", "pedidos", "promociones" }
}));

// =====================================================================
// CATEGORÍAS
// =====================================================================
app.MapGet("/api/categorias", () => Results.Ok(categorias));

app.MapGet("/api/categorias/{id:int}", (int id) =>
{
    var categoria = categorias.FirstOrDefault(c => c.Id == id);
    return categoria is not null
        ? Results.Ok(categoria)
        : Results.NotFound(new { message = "Categoría no encontrada." });
});

app.MapPost("/api/categorias", (Categoria nueva) =>
{
    if (string.IsNullOrWhiteSpace(nueva.Nombre))
        return Results.BadRequest(new { message = "El nombre de la categoría es obligatorio." });

    nueva.Id = siguienteIdCategoria++;
    categorias.Add(nueva);
    return Results.Created($"/api/categorias/{nueva.Id}", nueva);
});

app.MapPut("/api/categorias/{id:int}", (int id, Categoria actualizada) =>
{
    var categoria = categorias.FirstOrDefault(c => c.Id == id);
    if (categoria is null)
        return Results.NotFound(new { message = "Categoría no encontrada." });
    if (string.IsNullOrWhiteSpace(actualizada.Nombre))
        return Results.BadRequest(new { message = "El nombre de la categoría es obligatorio." });

    categoria.Nombre = actualizada.Nombre;
    categoria.Descripcion = actualizada.Descripcion;
    return Results.Ok(categoria);
});

app.MapDelete("/api/categorias/{id:int}", (int id) =>
{
    var categoria = categorias.FirstOrDefault(c => c.Id == id);
    if (categoria is null)
        return Results.NotFound(new { message = "Categoría no encontrada." });

    // Integridad referencial: no se puede borrar una categoría con productos.
    if (productos.Any(p => p.CategoriaId == id))
        return Results.Conflict(new { message = "No se puede eliminar: hay productos en esta categoría." });

    categorias.Remove(categoria);
    return Results.NoContent();
});

// =====================================================================
// PRODUCTOS  (recurso principal; el filtrado ocurre en el servidor
// mediante parámetros de consulta: ?categoriaId=&q=&genero=&min=&max=&soloOferta=)

app.MapGet("/api/productos", (int? categoriaId, string? q, string? genero, decimal? min, decimal? max, bool? soloOferta) =>
{
    IEnumerable<Producto> resultado = productos;

    if (categoriaId.HasValue)
        resultado = resultado.Where(p => p.CategoriaId == categoriaId.Value);

    if (!string.IsNullOrWhiteSpace(q))
    {
        var buscado = Texto.Normalizar(q);
        resultado = resultado.Where(p =>
            Texto.Normalizar(p.Nombre).Contains(buscado) ||
            Texto.Normalizar(p.Marca).Contains(buscado) ||
            Texto.Normalizar(p.Color).Contains(buscado));
    }

    if (!string.IsNullOrWhiteSpace(genero))
        resultado = resultado.Where(p => p.Genero.Equals(genero, StringComparison.OrdinalIgnoreCase));
    // El rango de precio se aplica sobre el precio FINAL (con descuento),
    // que es lo que el cliente realmente paga.
    if (min.HasValue)
        resultado = resultado.Where(p => p.PrecioFinal >= min.Value);

    if (max.HasValue)
        resultado = resultado.Where(p => p.PrecioFinal <= max.Value);

    if (soloOferta == true)
        resultado = resultado.Where(p => p.Descuento > 0);

    return Results.Ok(resultado.ToList());
});

app.MapGet("/api/productos/{id:int}", (int id) =>
{
    var producto = productos.FirstOrDefault(p => p.Id == id);
    return producto is not null
        ? Results.Ok(producto)
        : Results.NotFound(new { message = "Producto no encontrado." });
});

app.MapPost("/api/productos", (Producto nuevo) =>
{
    var error = ValidarProducto(nuevo, categorias);
    if (error is not null) return Results.BadRequest(new { message = error });

    nuevo.Id = siguienteIdProducto++;
    productos.Add(nuevo);
    return Results.Created($"/api/productos/{nuevo.Id}", nuevo);
});

app.MapPut("/api/productos/{id:int}", (int id, Producto actualizado) =>
{
    var producto = productos.FirstOrDefault(p => p.Id == id);
    if (producto is null)
        return Results.NotFound(new { message = "Producto no encontrado." });

    var error = ValidarProducto(actualizado, categorias);
    if (error is not null) return Results.BadRequest(new { message = error });

    producto.Codigo = actualizado.Codigo;
    producto.Nombre = actualizado.Nombre;
    producto.CategoriaId = actualizado.CategoriaId;
    producto.Genero = actualizado.Genero;
    producto.Talla = actualizado.Talla;
    producto.Color = actualizado.Color;
    producto.Precio = actualizado.Precio;
    producto.Stock = actualizado.Stock;
    producto.Marca = actualizado.Marca;
    producto.Material = actualizado.Material;
    producto.Temporada = actualizado.Temporada;
    producto.Descuento = actualizado.Descuento;
    producto.Imagen = actualizado.Imagen;
    producto.Descripcion = actualizado.Descripcion;
    return Results.Ok(producto);
});

app.MapDelete("/api/productos/{id:int}", (int id) =>
{
    var producto = productos.FirstOrDefault(p => p.Id == id);
    if (producto is null)
        return Results.NotFound(new { message = "Producto no encontrado." });

    productos.Remove(producto);
    return Results.Ok(new { mensaje = $"Producto con ID {id} eliminado correctamente", eliminado = producto });
});

// =====================================================================
// CLIENTES  (?correo= permite buscar un cliente por su correo)
// =====================================================================
app.MapGet("/api/clientes", (string? correo) =>
{
    IEnumerable<Cliente> resultado = clientes;

    if (!string.IsNullOrWhiteSpace(correo))
        resultado = resultado.Where(c => c.Correo.Equals(correo.Trim(), StringComparison.OrdinalIgnoreCase));

    return Results.Ok(resultado.ToList());
});

app.MapGet("/api/clientes/{id:int}", (int id) =>
{
    var cliente = clientes.FirstOrDefault(c => c.Id == id);
    return cliente is not null
        ? Results.Ok(cliente)
        : Results.NotFound(new { message = "Cliente no encontrado." });
});

app.MapPost("/api/clientes", (Cliente nuevo) =>
{
    if (string.IsNullOrWhiteSpace(nuevo.Nombre) || string.IsNullOrWhiteSpace(nuevo.Correo))
        return Results.BadRequest(new { message = "Nombre y correo son obligatorios." });

    if (!nuevo.Correo.Contains('@'))
        return Results.BadRequest(new { message = "El correo no tiene un formato válido." });

    if (clientes.Any(c => c.Correo.Equals(nuevo.Correo.Trim(), StringComparison.OrdinalIgnoreCase)))
        return Results.Conflict(new { message = "Ya existe un cliente con ese correo." });

    nuevo.Id = siguienteIdCliente++;
    nuevo.Correo = nuevo.Correo.Trim();
    clientes.Add(nuevo);
    return Results.Created($"/api/clientes/{nuevo.Id}", nuevo);
});

app.MapPut("/api/clientes/{id:int}", (int id, Cliente actualizado) =>
{
    var cliente = clientes.FirstOrDefault(c => c.Id == id);
    if (cliente is null)
        return Results.NotFound(new { message = "Cliente no encontrado." });
    if (string.IsNullOrWhiteSpace(actualizado.Nombre) || string.IsNullOrWhiteSpace(actualizado.Correo))
        return Results.BadRequest(new { message = "Nombre y correo son obligatorios." });

    cliente.Nombre = actualizado.Nombre;
    cliente.Correo = actualizado.Correo.Trim();
    cliente.Telefono = actualizado.Telefono;
    return Results.Ok(cliente);
});

app.MapDelete("/api/clientes/{id:int}", (int id) =>
{
    var cliente = clientes.FirstOrDefault(c => c.Id == id);
    if (cliente is null)
        return Results.NotFound(new { message = "Cliente no encontrado." });

    if (pedidos.Any(p => p.ClienteId == id))
        return Results.Conflict(new { message = "No se puede eliminar: el cliente tiene pedidos." });

    clientes.Remove(cliente);
    return Results.NoContent();
});

// =====================================================================
// PROMOCIONES  (?categoriaId= filtra las campañas de una categoría)
// =====================================================================
app.MapGet("/api/promociones", (int? categoriaId) =>
{
    IEnumerable<Promocion> resultado = promociones;

    if (categoriaId.HasValue)
        resultado = resultado.Where(p => p.CategoriaId == categoriaId.Value);

    return Results.Ok(resultado.ToList());
});

app.MapGet("/api/promociones/{id:int}", (int id) =>
{
    var promocion = promociones.FirstOrDefault(p => p.Id == id);
    return promocion is not null
        ? Results.Ok(promocion)
        : Results.NotFound(new { message = "Promoción no encontrada." });
});

app.MapPost("/api/promociones", (Promocion nueva) =>
{
    if (string.IsNullOrWhiteSpace(nueva.Titulo))
        return Results.BadRequest(new { message = "El título de la promoción es obligatorio." });
    if (nueva.Descuento < 0 || nueva.Descuento > 100)
        return Results.BadRequest(new { message = "El descuento debe estar entre 0 y 100." });
    if (!categorias.Any(c => c.Id == nueva.CategoriaId))
        return Results.BadRequest(new { message = "La categoría indicada no existe." });

    nueva.Id = siguienteIdPromocion++;
    promociones.Add(nueva);
    return Results.Created($"/api/promociones/{nueva.Id}", nueva);
});

app.MapDelete("/api/promociones/{id:int}", (int id) =>
{
    var promocion = promociones.FirstOrDefault(p => p.Id == id);
    if (promocion is null)
        return Results.NotFound(new { message = "Promoción no encontrada." });

    promociones.Remove(promocion);
    return Results.NoContent();
});

// =====================================================================
// PEDIDOS  (?clienteId= devuelve los pedidos de un cliente)
// =====================================================================
app.MapGet("/api/pedidos", (int? clienteId) =>
{
    IEnumerable<Pedido> resultado = pedidos;

    if (clienteId.HasValue)
        resultado = resultado.Where(p => p.ClienteId == clienteId.Value);

    return Results.Ok(resultado.ToList());
});

app.MapGet("/api/pedidos/{id:int}", (int id) =>
{
    var pedido = pedidos.FirstOrDefault(p => p.Id == id);
    return pedido is not null
        ? Results.Ok(pedido)
        : Results.NotFound(new { message = "Pedido no encontrado." });
});
app.MapPost("/api/pedidos", (NuevoPedido nuevo) =>
{
    if (nuevo.Items is null || nuevo.Items.Count == 0)
        return Results.BadRequest(new { message = "El pedido debe incluir al menos un producto." });

    if (!clientes.Any(c => c.Id == nuevo.ClienteId))
        return Results.BadRequest(new { message = "El cliente indicado no existe." });
    var solicitados = nuevo.Items
        .GroupBy(i => i.ProductoId)
        .Select(g => new NuevoItem(g.Key, g.Sum(i => i.Cantidad)))
        .ToList();

    lock (candado)
    {
        var items = new List<ItemPedido>();
        foreach (var solicitado in solicitados)
        {
            if (solicitado.Cantidad <= 0)
                return Results.BadRequest(new { message = "La cantidad de cada producto debe ser mayor a cero." });

            var producto = productos.FirstOrDefault(p => p.Id == solicitado.ProductoId);
            if (producto is null)
                return Results.BadRequest(new { message = $"El producto {solicitado.ProductoId} no existe." });

            if (producto.Stock < solicitado.Cantidad)
                return Results.BadRequest(new { message = $"Stock insuficiente de \"{producto.Nombre}\" (disponible: {producto.Stock})." });

            items.Add(new ItemPedido
            {
                ProductoId = producto.Id,
                Nombre = producto.Nombre,
                Cantidad = solicitado.Cantidad,
                PrecioUnitario = producto.PrecioFinal
            });
        }

        foreach (var item in items)
        {
            var producto = productos.First(p => p.Id == item.ProductoId);
            producto.Stock -= item.Cantidad;
        }

        var pedido = new Pedido
        {
            Id = siguienteIdPedido++,
            ClienteId = nuevo.ClienteId,
            // Perú usa UTC-5 todo el año (sin horario de verano).
            Fecha = DateTime.UtcNow.AddHours(-5).ToString("yyyy-MM-dd HH:mm"),
            Estado = "Pendiente",
            Items = items,
            Total = items.Sum(i => i.PrecioUnitario * i.Cantidad)
        };

        pedidos.Add(pedido);
        return Results.Created($"/api/pedidos/{pedido.Id}", pedido);
    }
});

// Cancelar un pedido devuelve las unidades al stock.
app.MapDelete("/api/pedidos/{id:int}", (int id) =>
{
    lock (candado)
    {
        var pedido = pedidos.FirstOrDefault(p => p.Id == id);
        if (pedido is null)
            return Results.NotFound(new { message = "Pedido no encontrado." });

        foreach (var item in pedido.Items)
        {
            var producto = productos.FirstOrDefault(p => p.Id == item.ProductoId);
            if (producto is not null) producto.Stock += item.Cantidad;
        }

        pedidos.Remove(pedido);
        return Results.NoContent();
    }
});

// Puerto: Render asigna la variable de entorno PORT en runtime.
// En local, si no existe, se usa 5180.
var port = Environment.GetEnvironmentVariable("PORT") ?? "5180";

app.Run($"http://0.0.0.0:{port}");

// =====================================================================
// Funciones auxiliares
// =====================================================================
static string? ValidarProducto(Producto p, List<Categoria> categorias)
{
    if (string.IsNullOrWhiteSpace(p.Nombre)) return "El nombre del producto es obligatorio.";
    if (p.Precio <= 0) return "El precio debe ser mayor a cero.";
    if (p.Stock < 0) return "El stock no puede ser negativo.";
    if (p.Descuento < 0 || p.Descuento > 100) return "El descuento debe estar entre 0 y 100.";
    if (!categorias.Any(c => c.Id == p.CategoriaId)) return "La categoría indicada no existe.";
    return null;
}

// =====================================================================
// Modelos. ASP.NET Core serializa a JSON en camelCase por defecto
// (CategoriaId -> "categoriaId"), listo para el frontend sin transformar.
// =====================================================================
class Categoria
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Descripcion { get; set; } = "";
}

class Producto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = "";
    public string Nombre { get; set; } = "";
    public int CategoriaId { get; set; }
    public string Genero { get; set; } = "";
    public string Talla { get; set; } = "";
    public string Color { get; set; } = "";
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string Marca { get; set; } = "";
    public string Material { get; set; } = "";
    public string Temporada { get; set; } = "";
    public int Descuento { get; set; }
    public string Imagen { get; set; } = "";
    public string Descripcion { get; set; } = "";

    // Propiedad calculada: el servidor es la única fuente de verdad del
    // precio final, así frontend y pedidos nunca difieren por redondeo.
    public decimal PrecioFinal =>
        Math.Round(Precio * (100 - Descuento) / 100m, 2, MidpointRounding.AwayFromZero);
}

class Cliente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Correo { get; set; } = "";
    public string Telefono { get; set; } = "";
}

class Promocion
{
    public int Id { get; set; }
    public string Titulo { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public int Descuento { get; set; }
    public int CategoriaId { get; set; }
    public string VigenteHasta { get; set; } = "";
}

class ItemPedido
{
    public int ProductoId { get; set; }
    public string Nombre { get; set; } = "";
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}

class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public string Fecha { get; set; } = "";
    public string Estado { get; set; } = "";
    public decimal Total { get; set; }
    public List<ItemPedido> Items { get; set; } = new();
}

// DTOs de entrada para crear un pedido (lo único que envía el cliente).
record NuevoPedido(int ClienteId, List<NuevoItem>? Items);
record NuevoItem(int ProductoId, int Cantidad);

static class Texto
{
    // Búsqueda sin distinguir mayúsculas ni tildes ("pantalon" encuentra
    // "Pantalón"). Se hace con reemplazos simples en vez de Normalize(FormD)
    // para no depender de ICU/globalización dentro del contenedor.
    public static string Normalizar(string texto) =>
        texto.ToLowerInvariant()
            .Replace('á', 'a').Replace('é', 'e').Replace('í', 'i')
            .Replace('ó', 'o').Replace('ú', 'u').Replace('ü', 'u')
            .Replace('ñ', 'n');
}
