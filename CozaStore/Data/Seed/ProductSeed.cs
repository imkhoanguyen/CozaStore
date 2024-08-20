using CozaStore.Helpers;
using CozaStore.Helpers.Enum;
using CozaStore.Models;
using System.Security.Policy;

namespace CozaStore.Data.Seed
{
    public static class ProductSeed
    {
        public static void Seed(DataContext context)
        {
            if (context.Products.Any()) return;
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Crinkle Jacket And Dress Set",
                    Price = 61,
                    PriceSell = 72,
                    Quantity = 151,
                    Status = (int)ProductStatus.Public,
                    IsFeatured = true,
                    Created = new DateTime(2023, 8, 20, 15, 30, 0),
                    Description = @"Size measurement: FREE size<br>
                                    Model info: 165cm<br>
                                    Color: GRAY<br><br>
                                    Care instructions: Not provided<br><br>
                                    Fabric info: Not provided",
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            ProductId = 1,
                            CategoryId = 1,
                        },
                        new ProductCategory
                        {
                            ProductId = 1,
                            CategoryId = 4,
                        }
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            Url = "/img/products/product-1.jpeg",
                            IsMain = true,
                        },
                        new Image
                        {
                            Url = "/img/products/product-1.jpg",
                            IsMain = false,
                        }

                    },
                    Variants = new List<Variant>
                    {
                        new Variant
                        {
                            ColorId = 3,
                            SizeId = 1,
                            Price = 61,
                            PriceSell = 72,
                            Quantity = 100,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 3,
                            SizeId = 2,
                            Price = 61,
                            PriceSell = 75,
                            Quantity = 51,
                            Status = (int)VariantStatus.Public,
                        }
                    }
                },

                //product2
                new Product
                {
                    Name = "Holding Two-way Zipper Hachi Knit Zip-up",
                    Price = 46,
                    PriceSell = 55,
                    Quantity = 208,
                    Status = (int)ProductStatus.Public,
                    IsFeatured = true,
                    Created = new DateTime(2023, 7, 20, 15, 30, 0),
                    Description = @"- Size: Free size (165cm)<br>
                                    - Fabric: Not specified<br>
                                    - Model info: Not provided<br>
                                    - Care instructions: Not provided",
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            ProductId = 2,
                            CategoryId = 1,
                        },
                        new ProductCategory
                        {
                            ProductId = 2,
                            CategoryId = 4,
                        },
                        new ProductCategory
                        {
                            ProductId = 2,
                            CategoryId = 9,
                        }
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            Url = "/img/products/product-2.jpeg",
                            IsMain = true,
                        },
                        new Image
                        {
                            Url = "/img/products/product-2.jpg",
                            IsMain = false,
                        }

                    },
                    Variants = new List<Variant>
                    {
                        new Variant
                        {
                            ColorId = 12,
                            SizeId = 1,
                            Price = 46,
                            PriceSell = 55,
                            Quantity = 52,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 3,
                            SizeId = 2,
                            Price = 47,
                            PriceSell = 60,
                            Quantity = 52,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 12,
                            SizeId = 2,
                            Price = 46,
                            PriceSell = 58,
                            Quantity = 52,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 3,
                            SizeId = 1,
                            Price = 49,
                            PriceSell = 70,
                            Quantity = 52,
                            Status = (int)VariantStatus.Public,
                        }
                    }
                },

                //product3
                new Product
                {
                    Name = "Tone-on-tone Delicate Summer Shirt",
                    Price = 50,
                    PriceSell = 58,
                    Quantity = 160,
                    Status = (int)ProductStatus.Public,
                    IsFeatured = true,
                    Created = new DateTime(2023, 7, 20, 15, 30, 0),
                    Description = @"Size: FREE (fits 168cm height), Top: 55, Bottom: 26<br>
                                    Fabric: Not provided<br>
                                    Model info: Not provided<br>
                                    Care instructions: Not provided",
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            ProductId = 3,
                            CategoryId = 1,
                        },
                        new ProductCategory
                        {
                            ProductId = 3,
                            CategoryId = 4,
                        },
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            Url = "/img/products/product-3.jpeg",
                            IsMain = true,
                        },
                        new Image
                        {
                            Url = "/img/products/product-3.jpg",
                            IsMain = false,
                        }

                    },
                    Variants = new List<Variant>
                    {
                        new Variant
                        {
                            ColorId = 12,
                            SizeId = 1,
                            Price = 50,
                            PriceSell = 58,
                            Quantity = 40,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 13,
                            SizeId = 1,
                            Price = 50,
                            PriceSell = 60,
                            Quantity = 40,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 12,
                            SizeId = 2,
                            Price = 50,
                            PriceSell = 58,
                            Quantity = 40,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 13,
                            SizeId = 3,
                            Price = 55,
                            PriceSell = 65,
                            Quantity = 40,
                            Status = (int)VariantStatus.Public,
                        }
                    }
                },

                //product4
                new Product
                {
                    Name = "Áo Thun ED Stripe Artwork ( Semi-Overfit )",
                    Price = 10,
                    PriceSell = 20,
                    Quantity = 200,
                    Status = (int)ProductStatus.Public,
                    IsFeatured = true,
                    Created = DateTime.UtcNow,
                    Description = @"Thêm sự lựa chọn cho các 💟 Couple.<br>
                                    Stripe Artwork Short Sleeve T-shirt 🇰🇷<br>
                                    ▪️ Chất liệu vải: Cotton + Poly giữ form tốt.<br>
                                    ▪️ Bề mặt vải được xử lý thoáng mát, êm mịn.<br>
                                    ▪️ Form Semi-Overfit rộng nhẹ, thoải mái.",
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            ProductId = 4,
                            CategoryId = 1,
                        },
                        new ProductCategory
                        {
                            ProductId = 4,
                            CategoryId = 4,
                        },
                        new ProductCategory
                        {
                            ProductId = 4,
                            CategoryId = 2,
                        },
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            Url = "/img/products/product-4-1.jpg",
                            IsMain = true,
                        },
                        new Image
                        {
                            Url = "/img/products/product-4-2.jpg",
                            IsMain = false,
                        }

                    },
                    Variants = new List<Variant>
                    {
                        new Variant
                        {
                            ColorId = 4,
                            SizeId = 5,
                            Price = 50,
                            PriceSell = 58,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 12,
                            SizeId = 5,
                            Price = 10,
                            PriceSell = 12,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 8,
                            SizeId = 5,
                            Price = 10,
                            PriceSell = 13,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 8,
                            SizeId = 4,
                            Price = 10,
                            PriceSell = 10,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        }
                    }
                },


                //product5
                new Product
                {
                    Name = "Áo Thun DB Lightwork Symbol ( Graphic )",
                    Price = 10,
                    PriceSell = 25,
                    Quantity = 200,
                    Status = (int)ProductStatus.Public,
                    IsFeatured = true,
                    Created = DateTime.UtcNow,
                    Description = @"Lightwork Symbol Graphic S/S T-Shirt 🇰🇷<br>
                                [ DB2407 ]<br>
                                ▪️ Sử dụng chất liệu giúp tối ưu hóa việc thoát mồ hôi.<br>
                                ▪️ Chất liệu: thun poly spandex mịn, co giãn 4 chiều tăng khả năng đàn hồi tốt.<br>
                                ▪️ Sản phẩm thoáng mát, nhanh khô, êm mịn khi tiếp xúc da.",
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            ProductId = 5,
                            CategoryId = 1,
                        },
                        new ProductCategory
                        {
                            ProductId = 5,
                            CategoryId = 4,
                        },
                        new ProductCategory
                        {
                            ProductId = 5,
                            CategoryId = 2,
                        },
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            Url = "/img/products/product-5.jpg",
                            IsMain = true,
                        },
                        new Image
                        {
                            Url = "/img/products/product-5-1.jpg",
                            IsMain = false,
                        }

                    },
                    Variants = new List<Variant>
                    {
                        new Variant
                        {
                            ColorId = 9,
                            SizeId = 5,
                            Price = 50,
                            PriceSell = 58,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 12,
                            SizeId = 5,
                            Price = 10,
                            PriceSell = 12,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 8,
                            SizeId = 5,
                            Price = 10,
                            PriceSell = 13,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 8,
                            SizeId = 4,
                            Price = 10,
                            PriceSell = 10,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        }
                    }
                },

                //product6
                new Product
                {
                    Name = "Áo Khoác UB ( Windbreaker )",
                    Price = 30,
                    PriceSell = 50,
                    Quantity = 200,
                    Status = (int)ProductStatus.Public,
                    IsFeatured = true,
                    Created = DateTime.UtcNow,
                    Description = @"2-WAY Windbreaker Jacket 🇰🇷<br>
                                    [ UB2408 ]<br>
                                    ▪️ Phần cổ cao chống bám bụi bẩn, cản gió, nhanh khô.<br>
                                    ▪️ Chất liệu: Nylon + Poly nhanh khô, thoáng khí.<br>
                                    ▪️ Form áo rộng, thoải mái kết hợp khóa khéo 2 chiều dễ dàng tùy chỉnh.",
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            ProductId = 6,
                            CategoryId = 1,
                        },
                        new ProductCategory
                        {
                            ProductId = 6,
                            CategoryId = 4,
                        },
                        new ProductCategory
                        {
                            ProductId = 6,
                            CategoryId = 2,
                        },
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            Url = "/img/products/product-6.jpg",
                            IsMain = true,
                        },
                        new Image
                        {
                            Url = "/img/products/product-6-1.jpg",
                            IsMain = false,
                        }

                    },
                    Variants = new List<Variant>
                    {
                        new Variant
                        {
                            ColorId = 1,
                            SizeId = 5,
                            Price = 32,
                            PriceSell = 60,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 3,
                            SizeId = 5,
                            Price = 32,
                            PriceSell = 60,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 1,
                            SizeId = 4,
                            Price = 30,
                            PriceSell = 50,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 3,
                            SizeId = 4,
                            Price = 30,
                            PriceSell = 50,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        }
                    }
                },


                //product7
                new Product
                {
                    Name = "Áo Thun AEC Clover Tiger ( Overfit )",
                    Price = 15,
                    PriceSell = 22,
                    Quantity = 200,
                    Status = (int)ProductStatus.Public,
                    IsFeatured = true,
                    Created = DateTime.UtcNow,
                    Description = @"Clover Tiger Short Sleeve T-shirt 🇰🇷<br>
                                [ AEC2401 ]<br>
                                ▪️ Chất liệu vải : Heavy cotton dày dặn, thoát hơi tốt, nhanh khô<br>
                                ▪️ Form Overfit rộng, thoải mái.",
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            ProductId = 7,
                            CategoryId = 1,
                        },
                        new ProductCategory
                        {
                            ProductId = 7,
                            CategoryId = 4,
                        },
                        new ProductCategory
                        {
                            ProductId = 7,
                            CategoryId = 2,
                        },
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            Url = "/img/products/product-7.jpg",
                            IsMain = true,
                        },
                        new Image
                        {
                            Url = "/img/products/product-7-1.jpg",
                            IsMain = false,
                        }

                    },
                    Variants = new List<Variant>
                    {
                        new Variant
                        {
                            ColorId = 2,
                            SizeId = 5,
                            Price = 15,
                            PriceSell = 22,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 2,
                            SizeId = 5,
                            Price = 15,
                            PriceSell = 22,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 2,
                            SizeId = 4,
                            Price = 15,
                            PriceSell = 22,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 2,
                            SizeId = 4,
                            Price = 15,
                            PriceSell = 22,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        }
                    }
                },


                //product8
                new Product
                {
                    Name = "Mole Pigment T-shirt",
                    Price = 30,
                    PriceSell = 35,
                    Quantity = 200,
                    Status = (int)ProductStatus.Public,
                    IsFeatured = true,
                    Created = DateTime.UtcNow,
                    Description = @"- Size: FREE size<br>
                                    - Model height: 165cm<br>
                                    - Care instructions: Not provided",
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            ProductId = 8,
                            CategoryId = 1,
                        },
                        new ProductCategory
                        {
                            ProductId = 8,
                            CategoryId = 4,
                        },
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            Url = "/img/products/product-8.jpeg",
                            IsMain = true,
                        },
                        new Image
                        {
                            Url = "/img/products/product-8.jpg",
                            IsMain = false,
                        }

                    },
                    Variants = new List<Variant>
                    {
                        new Variant
                        {
                            ColorId = 5,
                            SizeId = 5,
                            Price = 30,
                            PriceSell = 35,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 5,
                            SizeId = 4,
                            Price = 30,
                            PriceSell = 35,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 5,
                            SizeId = 3,
                            Price = 30,
                            PriceSell = 22,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 5,
                            SizeId = 2,
                            Price = 35,
                            PriceSell = 22,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        }
                    }
                },

                //product9
                new Product
                {
                    Name = "Gracia Long Sleeve Pair",
                    Price = 60,
                    PriceSell = 80,
                    Quantity = 200,
                    Status = (int)ProductStatus.Public,
                    IsFeatured = true,
                    Created = DateTime.UtcNow,
                    Description = @"Fabric/Material<br>
                                    Cotton 57%, Modal 43%",
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            ProductId = 9,
                            CategoryId = 1,
                        },
                        new ProductCategory
                        {
                            ProductId = 9,
                            CategoryId = 4,
                        },
                        new ProductCategory
                        {
                            ProductId = 9,
                            CategoryId = 2,
                        },
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            Url = "/img/products/product-9.jpeg",
                            IsMain = true,
                        },
                        new Image
                        {
                            Url = "/img/products/product-9.jpg",
                            IsMain = false,
                        }

                    },
                    Variants = new List<Variant>
                    {
                        new Variant
                        {
                            ColorId = 2,
                            SizeId = 5,
                            Price = 60,
                            PriceSell = 82,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 2,
                            SizeId = 4,
                            Price = 60,
                            PriceSell = 80,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 2,
                            SizeId = 3,
                            Price = 60,
                            PriceSell = 80,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 2,
                            SizeId = 2,
                            Price = 60,
                            PriceSell = 80,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        }
                    }
                },


                //product10
                new Product
                {
                    Name = "Fun Game Printing Short Sleeve T-shirt",
                    Price = 38,
                    PriceSell = 45,
                    Quantity = 200,
                    Status = (int)ProductStatus.Public,
                    IsFeatured = true,
                    Created = DateTime.UtcNow,
                    Description = @"- Size: FREE size<br>
                                    - Fabric: 100% cotton<br>
                                    - Model: Y모델 / Free size<br>
                                    - Care instructions: Hand wash, dry cleaning, wash separately, use dedicated cleaner, do not wash with water",
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            ProductId = 10,
                            CategoryId = 1,
                        },
                        new ProductCategory
                        {
                            ProductId = 10,
                            CategoryId = 4,
                        },
                        new ProductCategory
                        {
                            ProductId = 10,
                            CategoryId = 2,
                        },
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            Url = "/img/products/product-10.jpeg",
                            IsMain = true,
                        },
                        new Image
                        {
                            Url = "/img/products/product-10.jpg",
                            IsMain = false,
                        }

                    },
                    Variants = new List<Variant>
                    {
                        new Variant
                        {
                            ColorId = 1,
                            SizeId = 5,
                            Price = 38,
                            PriceSell = 47,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 1,
                            SizeId = 4,
                            Price = 38,
                            PriceSell = 45,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 1,
                            SizeId = 3,
                            Price = 38,
                            PriceSell = 45,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 1,
                            SizeId = 2,
                            Price = 38,
                            PriceSell = 45,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        }
                    }
                },


                //product11
                new Product
                {
                    Name = "Unisex Hanbok Gogori Jeogori",
                    Price = 166,
                    PriceSell = 184,
                    Quantity = 200,
                    Status = (int)ProductStatus.Public,
                    IsFeatured = true,
                    Created = DateTime.UtcNow,
                    Description = @"Fabric/Material<br>
                                    Cotton 100%<br>
                                    Our Model Stats<br>
                                    - Height: 5'4"" / 163cm<br>
                                    - Waist: 24"" / 60cm<br>
                                    - Weight: NA<br>
                                    - Model typically wears size XS/S<br>
                                    Model is wearing S<br>
                                    Care Info<br>
                                    Dry Clean, Hand Wash, Machine Wash",
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            ProductId = 11,
                            CategoryId = 1,
                        },
                        new ProductCategory
                        {
                            ProductId = 11,
                            CategoryId = 4,
                        },
                        new ProductCategory
                        {
                            ProductId = 11,
                            CategoryId = 2,
                        },
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            Url = "/img/products/product-11.jpg",
                            IsMain = true,
                        },
                        new Image
                        {
                            Url = "/img/products/product-11-1.jpg",
                            IsMain = false,
                        }

                    },
                    Variants = new List<Variant>
                    {
                        new Variant
                        {
                            ColorId = 12,
                            SizeId = 5,
                            Price = 166,
                            PriceSell = 190,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 12,
                            SizeId = 4,
                            Price = 166,
                            PriceSell = 190,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 8,
                            SizeId = 4,
                            Price = 166,
                            PriceSell = 184,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 8,
                            SizeId = 5,
                            Price = 166,
                            PriceSell = 184,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        }
                    }
                },

                //product12
                new Product
                {
                    Name = "Berry Lettering Printed Short Sleeve Sweatshirt",
                    Price = 35,
                    PriceSell = 40,
                    Quantity = 200,
                    Status = (int)ProductStatus.Public,
                    IsFeatured = true,
                    Created = DateTime.UtcNow,
                    Description = @"Size measurement: FREE size 165cm (height), Top 55, Bottom 26<br>
                                    Fabric info: Not provided<br>
                                    Model info: Not provided<br>
                                    Care instructions: Not provided<br>",
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            ProductId = 12,
                            CategoryId = 1,
                        },
                        new ProductCategory
                        {
                            ProductId = 12,
                            CategoryId = 4,
                        },
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            Url = "/img/products/product-12.jpeg",
                            IsMain = true,
                        },
                        new Image
                        {
                            Url = "/img/products/product-12.jpg",
                            IsMain = false,
                        }

                    },
                    Variants = new List<Variant>
                    {
                        new Variant
                        {
                            ColorId = 1,
                            SizeId = 4,
                            Price = 35,
                            PriceSell = 40,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 1,
                            SizeId = 5,
                            Price = 35,
                            PriceSell = 40,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 2,
                            SizeId = 5,
                            Price = 35,
                            PriceSell = 40,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 2,
                            SizeId = 4,
                            Price = 38,
                            PriceSell = 45,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        }
                    }
                },

                //product13
                new Product
                {
                    Name = "Moment Embroidered String Sweatshirt",
                    Price = 30,
                    PriceSell = 35,
                    Quantity = 200,
                    Status = (int)ProductStatus.Public,
                    IsFeatured = true,
                    Created = DateTime.UtcNow,
                    Description = @"- Size: FREE size<br>
                                    - Model height: 165cm<br>
                                    - Top size: 55<br>
                                    - Bottom size: 26",
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            ProductId = 13,
                            CategoryId = 1,
                        },
                        new ProductCategory
                        {
                            ProductId = 13,
                            CategoryId = 4,
                        },
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            Url = "/img/products/product-13.jpeg",
                            IsMain = true,
                        },
                        new Image
                        {
                            Url = "/img/products/product-13.jpg",
                            IsMain = false,
                        }

                    },
                    Variants = new List<Variant>
                    {
                        new Variant
                        {
                            ColorId = 2,
                            SizeId = 5,
                            Price = 30,
                            PriceSell = 40,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 2,
                            SizeId = 4,
                            Price = 30,
                            PriceSell = 40,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 8,
                            SizeId = 5,
                            Price = 30,
                            PriceSell = 35,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 8,
                            SizeId = 4,
                            Price = 30,
                            PriceSell = 35,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        }
                    }
                },

                //product14
                new Product
                {
                    Name = "Unisex Dragon Varsity Jacket NY Yankees Black",
                    Price = 400,
                    PriceSell = 450,
                    Quantity = 200,
                    Status = (int)ProductStatus.Public,
                    IsFeatured = true,
                    Created = DateTime.UtcNow,
                    Description = @"Fabric/Material<br>
                                    Polyester 100%<br>
                                    Care Info<br>
                                    Dry Clean, Hand Wash, Cold Wash, Machine Wash",
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            ProductId = 14,
                            CategoryId = 1,
                        },
                        new ProductCategory
                        {
                            ProductId = 14,
                            CategoryId = 4,
                        },
                        new ProductCategory
                        {
                            ProductId = 14,
                            CategoryId = 2,
                        },
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            Url = "/img/products/product-14.jpg",
                            IsMain = true,
                        },
                        new Image
                        {
                            Url = "/img/products/product-14-1.jpg",
                            IsMain = false,
                        }

                    },
                    Variants = new List<Variant>
                    {
                        new Variant
                        {
                            ColorId = 1,
                            SizeId = 5,
                            Price = 400,
                            PriceSell = 450,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 1,
                            SizeId = 4,
                            Price = 400,
                            PriceSell = 450,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 8,
                            SizeId = 3,
                            Price = 400,
                            PriceSell = 450,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 1,
                            SizeId = 2,
                            Price = 400,
                            PriceSell = 450,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        }
                    }
                },


                //product15
                new Product
                {
                    Name = "Unisex Classic Monogram Big Luxe Short Sleeve Tee Shirt Boston Redsox Cream",
                    Price = 100,
                    PriceSell = 110,
                    Quantity = 200,
                    Status = (int)ProductStatus.Public,
                    IsFeatured = true,
                    Created = DateTime.UtcNow,
                    Description = @"Fabric/Material<br>
                                    Cotton 100%<br>
                                    Care Info<br>
                                    Hand Wash, Machine Wash",
                    ProductCategories = new List<ProductCategory>
                    {
                        new ProductCategory
                        {
                            ProductId = 15,
                            CategoryId = 1,
                        },
                        new ProductCategory
                        {
                            ProductId = 15,
                            CategoryId = 4,
                        },
                        new ProductCategory
                        {
                            ProductId = 15,
                            CategoryId = 2,
                        },
                    },
                    Images = new List<Image>
                    {
                        new Image
                        {
                            Url = "/img/products/product-15.jpg",
                            IsMain = true,
                        },
                        new Image
                        {
                            Url = "/img/products/product-15-1.jpg",
                            IsMain = false,
                        }

                    },
                    Variants = new List<Variant>
                    {
                        new Variant
                        {
                            ColorId = 2,
                            SizeId = 5,
                            Price = 100,
                            PriceSell = 112,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 2,
                            SizeId = 4,
                            Price = 100,
                            PriceSell = 110,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 2,
                            SizeId = 3,
                            Price = 100,
                            PriceSell = 110,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        },
                        new Variant
                        {
                            ColorId = 2,
                            SizeId = 2,
                            Price = 100,
                            PriceSell = 110,
                            Quantity = 50,
                            Status = (int)VariantStatus.Public,
                        }
                    }
                },
            };

            context.Products.AddRange(products);
            context.SaveChanges();
            
        }
    }
}
