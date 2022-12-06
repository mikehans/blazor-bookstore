﻿using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.Book;

public class BookCreateDTO
{
    [Required]
    [StringLength(50)]
    public string Title { get; set; }
    [Required]
    public int Year { get; set; }
    [Required]
    [StringLength(50)]
    public string Isbn { get; set; } = null!;
    [StringLength(250)]
    public string? Summary { get; set; }
    [StringLength(50)]
    public string? Image { get; set; }
    public decimal? Price { get; set; }
    public int AuthorId { get; set; }
}
