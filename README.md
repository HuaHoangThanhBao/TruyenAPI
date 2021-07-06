# TruyenAPI

Rest API was built with ASP.NET Core 3.0 with CRUD operations for a reading book website.
It is using Repository pattern to design architecture and communicating with database by
ORM model.

## Set-up

1. Open your project -> Change your server name in appsetting.Body (Json) -> Open Window console
2. Typing 'Add-migration create-database' command to create Migrations folder.
3. Typing 'Update-database' to create database in SQL Server.

## API (HTTP request)

Http-Headers:

Each request must be attached with HttpHeaders and we need declare APIKey inside HttpHeaders. Because of security, 
APIKey would not be in Git.

Example:

````console

headers: {
  "Content-Type": "application/json",
  "Api-Key": environment.apiKey
}

````

Note:
1. [1] - Get all records
2. [2] - Get only one record.
3. [3] - Get record with foreign key.
4. {id}: Id of record which you want to get.

#### TacGia table

````console

Method: POST

Request: host/api/tacgia
Body (Json):
[
  {
    "TenTacGia": string,
    "TinhTrang": boolean
  }
  ,...
]

````

````console

Method: GET

[1] - Request: host/api/tacgia

[2] - Request: host/api/tacgia/{id}

[3] - Request: host/api/tacgia/{id}/details

````

````console

Method: PUT

Request: host/api/tacgia
Body (Json):
{
    "TenTacGia": string,
    "TinhTrang": boolean
}

````

````console

Method: DELETE

Request: Request/api/tacgia/{id}

````

#### TheLoai table

````console

Method: POST

Request: host/api/theloai
Body (Json):
[
  {
    "TenTheLoai": string,
    "TinhTrang": boolean
  }
  ,...
]

````

````console

Method: GET

[1] - Request: host/api/theloai

[2] - Request: host/api/theloai/{id}

[3] - Request: host/api/theloai/{id}/details

````

````console

Method: PUT

Request: host/api/theloai
Body (Json):
{
    "TenTheLoai": string,
    "TinhTrang": boolean
}

````

````console

Method: DELETE

Request: Request/api/theloai/{id}

````

#### Truyen table

````console

Method: POST

Request: host/api/truyen
Body (Json):
[
  {
    "TacGiaID": number,
    "TenTruyen": string,
    "MoTa": string,
    "TinhTrang": boolean,
    "HinhAnh": imageRequest
  }
  ,...
]

````

````console

Method: GET

[1] - Request: host/api/truyen

[2] - Request: host/api/truyen/{id}

[3] - Request: host/api/truyen/{id}/details

````

````console

Method: PUT

Request: host/api/truyen
Body (Json):
{
    "TacGiaID": number,
    "TenTruyen": string,
    "MoTa": string,
    "TinhTrang": boolean,
    "HinhAnh": imageRequest
}

````

````console

Method: DELETE

Request: Request/api/truyen/{id}

````

#### PhuLuc table

````console

Method: POST

Request: host/api/phuluc
Body (Json):
[
  {
    "TruyenID": number,
    "TheLoaiID": number
  }
  ,...
]

````

````console

Method: GET

[1] - Request: host/api/phuluc

[2] - Request: host/api/phuluc/{id}

````

````console

Method: PUT

Request: host/api/phuluc
Body (Json):
{
    "TruyenID": number,
    "TheLoaiID": number
}

````

````console

Method: DELETE

Request: Request/api/phuluc/{id}

````

#### Chuong table

````console

Method: POST

Request: host/api/chuong
Body (Json):
[
  {
    "TruyenID": number,
    "TenChuong": string,
    "ThoiGianCapNhat": datetime,
    "LuotXem": number
  }
  ,...
]

````

````console

Method: GET

[1] - Request: host/api/chuong

[2] - Request: host/api/chuong/{id}

[3] - Request: host/api/chuong/{id}/details

````

````console

Method: PUT

Request: host/api/chuong
Body (Json):
{
   "TruyenID": number,
   "TenChuong": string,
   "ThoiGianCapNhat": datetime,
   "LuotXem": number
}

````

````console

Method: DELETE

Request: Request/api/chuong/{id}

````

#### NoiDungChuong table

````console

Method: POST

Request: host/api/noidungchuong
Body (Json):
[
  {
     "ChuongID": number,
     "HinhAnh": imageRequest
  }
  ,...
]

````

````console

Method: GET

[1] - Request: host/api/noidungchuong

[2] - Request: host/api/noidungchuong/{id}

[3] - Request: host/api/noidungchuong/{id}/details

````

````console

Method: PUT

Request: host/api/noidungchuong
Body (Json):
{
   "ChuongID": number,
   "HinhAnh": imageRequest
}

````

````console

Method: DELETE

Request: Request/api/noidungchuong/{id}

````

#### User table

````console

Method: POST

Request: host/api/user
Body (Json):
{
    "TenUser": string,
    "Password": string
}

````

````console

Method: GET

[1] - Request: host/api/user

[2] - Request: host/api/user/{id}

[3] - Request: host/api/user/{id}/details

````

````console

Method: PUT

Request: host/api/user
Body (Json):
{
    "TenUser": string,
    "Quyen": number,
    "Password": string
}

````

````console

Method: DELETE

Request: Request/api/user/{id}

````

#### TheoDoi table

````console

Method: POST

Request: host/api/theodoi
Body (Json):
{
    "TruyenID": string,
    "Quyen": number,
    "UserID": string
}

````

````console

Method: GET

[1] - Request: host/api/theodoi

[2] - Request: host/api/theodoi/{id}

[3] - Request: host/api/theodoi/{id}/details

````

````console

Method: PUT

Request: host/api/theodoi
Body (Json):
{
    "TruyenID": string,
    "UserID": string
}

````

````console

Method: DELETE

Request: Request/api/theodoi/{id}

````

#### BinhLuan table

````console

Method: POST

Request: host/api/binhluan
Body (Json):
{
    "UserID": string,
    "ChuongID": number,
    "NoiDung": string,
    "NgayBL": datetime
}

````

````console

Method: GET

[1] - Request: host/api/binhluan

[2] - Request: host/api/binhluan/{id}

[3] - Request: host/api/binhluan/{id}/details

````

````console

Method: PUT

Request: host/api/binhluan
Body (Json):
{
    "UserID": string,
    "ChuongID": number,
    "NoiDung": string,
    "NgayBL": datetime
}

````

````console

Method: DELETE

Request: Request/api/binhluan/{id}

````

#### JWT For Login

````console

Method: POST

Request: host/api/auth/login
Body (Json):
{
    "TenUser": string,
    "Password": string
}

````

#### Pagination

1. Truyen table

````console

Method: GET

[Get all]: host/api/truyen?pageNumber={number}&pageSize={number}&getAll=true

[Get lastest updates]: host/api/truyen?pageNumber={number}&pageSize={number}&lastestUpdate=true

[Get top views]: host/api/truyen?pageNumber={number}&pageSize={number}&topView=true

[Get truyen of theloai]: host/api/truyen?pageNumber={number}&pageSize={number}&sorting=true&theloaiID={id}

[Get truyen of theodoi]: host/api/truyen?pageNumber={number}&pageSize={number}&sorting=true&userID={id}

````

2. BinhLuan table

````console

Method: GET

[Get all]: host/api/binhluan?pageNumber={number}&pageSize={number}&getAll=true

[Get lastest updates]: host/api/binhluan?pageNumber={number}&pageSize={number}&lastestUpdate=true

[Get top 10 lastest updates]: host/api/binhluan?pageNumber={number}&pageSize={number}&sorting=true&truyenID={id}

[Get binhluan of chuong]: host/api/binhluan?pageNumber={number}&pageSize={number}&sorting=true&chuongID={id}

````