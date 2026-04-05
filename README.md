<div id="top"></div>

<div align="center">

# StockKeeperMail

Настольное приложение для управления складом, заказами, товарами, ролями пользователей и внутренними бизнес-процессами

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![WPF](https://img.shields.io/badge/WPF-Windows%20Desktop-5C2D91?style=for-the-badge&logo=windows&logoColor=white)
![XAML](https://img.shields.io/badge/XAML-0C54C2?style=for-the-badge&logo=xaml&logoColor=white)
![EF Core](https://img.shields.io/badge/EF_Core-SQL_Server-6DB33F?style=for-the-badge&logo=.net&logoColor=white)
![Material Design](https://img.shields.io/badge/Material_Design_in_XAML-757575?style=for-the-badge&logo=materialdesign&logoColor=white)
![MVVM](https://img.shields.io/badge/MVVM-CommunityToolkit-0A84FF?style=for-the-badge)

</div>

---

## Содержание

- [О проекте](#о-проекте)
- [Ключевые возможности](#ключевые-возможности)
- [Скриншоты интерфейса](#скриншоты-интерфейса)
- [Технологический стек](#технологический-стек)
- [Используемые библиотеки](#используемые-библиотеки)
- [Архитектура решения](#архитектура-решения)
- [Структура проекта](#структура-проекта)
- [Быстрый старт](#быстрый-старт)
- [Настройка базы данных](#настройка-базы-данных)
- [XML / Summary-документация](#xml--summary-документация)
- [Планы по развитию](#планы-по-развитию)

## О проекте

**StockKeeperMail** — это настольная система складского учета, которая объединяет в одном приложении работу с товарами, категориями, поставщиками, заказами, клиентами, сотрудниками, ролями, складскими локациями, внутренними сообщениями и журналом действий.

В проекте сделан упор не только на базовые CRUD-операции, но и на реальные пользовательские сценарии: авторизацию, разграничение доступа, управление остатками, оформление заказа с позициями, обновление статусов доставки, печать накладной, учет бракованных товаров и аудит действий сотрудников.

> [!IMPORTANT]
> Проект состоит из двух основных модулей:
> - **StockKeeperMail.Desktop** — клиентская часть на WPF
> - **StockKeeperMail.Database** — слой данных, моделей, миграций и конфигурации подключения

## Ключевые возможности

### Авторизация и разграничение прав
- вход сотрудников в систему через отдельное окно авторизации
- ролевая модель доступа
- управление разрешениями на просмотр, создание, изменение и удаление данных
- ограничение доступа к разделам системы в зависимости от роли пользователя

### Панель управления и аналитика
- стартовый экран с ключевыми показателями системы
- карточки со статистикой по заказам и товарам
- графики и показатели по выручке
- быстрый обзор текущего состояния склада и заказов

### Управление товарами и каталогом
- создание, редактирование и удаление товаров
- работа с категориями, поставщиками и характеристиками товара
- контроль цены, количества и доступности
- привязка товара к складской локации

### Работа со складом
- управление складскими локациями
- учет размещения товара
- обработка списаний и бракованной продукции
- обновление остатков при операциях с заказами и товарами

### Работа с заказами
- создание и редактирование заказов
- добавление позиций заказа
- расчет итоговой суммы
- управление статусами доставки
- формирование и печать накладной

### Управление пользователями и коммуникацией
- ведение базы сотрудников и ролей
- работа с клиентами
- внутренние сообщения между сотрудниками
- журналирование действий в системе

## Скриншоты интерфейса

> [!NOTE]
> README рассчитан на изображения в папке **`images`** со следующими именами:
> `LoginView.png`, `MainWindow.png`, `CategoryListView.png`, `RoleFormView.png`, `PrintInvoiceView.png`

### LoginView
![LoginView](images/LoginView.png)

### MainWindow
![MainWindow](images/MainWindow.png)

### CategoryListView
![CategoryListView](images/CategoryListView.png)

### RoleFormView
![RoleFormView](images/RoleFormView.png)

### PrintInvoiceView
![PrintInvoiceView](images/PrintInvoiceView.png)

## Технологический стек

- **Язык программирования:** C#
- **Платформа:** .NET 10 Windows
- **Пользовательский интерфейс:** WPF + XAML
- **Архитектурный подход:** MVVM
- **СУБД:** Microsoft SQL Server / LocalDB
- **ORM:** Entity Framework Core
- **Конфигурация:** JSON + Microsoft.Extensions.Configuration

## Используемые библиотеки

- [CommunityToolkit.Mvvm](https://www.nuget.org/packages/CommunityToolkit.Mvvm/) — реализация MVVM, команды, уведомление об изменениях, вспомогательные базовые классы
- [MaterialDesignThemes](https://www.nuget.org/packages/MaterialDesignThemes/) — оформление интерфейса в стиле Material Design для WPF
- [MaterialDesignColors](https://www.nuget.org/packages/MaterialDesignColors/) — палитры и цветовые ресурсы для Material Design
- [LiveCharts.Wpf.Core](https://www.nuget.org/packages/LiveCharts.Wpf.Core/) — графики и визуализация аналитических данных
- [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/) — работа с SQL Server через EF Core
- [Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/) — миграции и команды EF Core
- [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration/) — доступ к конфигурации приложения
- [Microsoft.Extensions.Configuration.Json](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Json/) — загрузка конфигурации из JSON
- [Microsoft.Extensions.Hosting](https://www.nuget.org/packages/Microsoft.Extensions.Hosting/) — инфраструктурные зависимости и базовая организация приложения
- [Windows Presentation Foundation (WPF)](https://learn.microsoft.com/dotnet/desktop/wpf/) — платформа построения настольного интерфейса

## Архитектура решения

Решение построено по модульному принципу и разделено на клиентскую и серверную часть внутри одного solution.

### StockKeeperMail.Desktop
- представления `Views`
- представления моделей `ViewModels`
- пользовательские контролы `Controls`
- навигация и состояние приложения `Stores`
- сервисы и DAL-слой
- словари ресурсов и стили интерфейса

### StockKeeperMail.Database
- сущности предметной области
- `InventoryManagementContext`
- миграции базы данных
- строка подключения в `dbconfig.json`

> [!TIP]
> Такая структура удобна для сопровождения проекта, потому что UI-логика, бизнес-сценарии и слой данных разделены по ответственности, а не смешаны в одном модуле

## Структура проекта

```text
StockKeeperMail/
├── StockKeeperMail.Desktop/
│   ├── Assets/
│   ├── Controls/
│   ├── Converters/
│   ├── DAL/
│   ├── ResourceDictionaries/
│   ├── Services/
│   ├── Stores/
│   ├── Utilities/
│   ├── ViewModels/
│   └── Views/
├── StockKeeperMail.Database/
│   ├── Data/
│   ├── Migrations/
│   ├── Models/
│   ├── Services/
│   └── dbconfig.json
└── StockKeeperMail.slnx
```

## Быстрый старт

### Требования

- Visual Studio 2022
- .NET 10 SDK
- Microsoft SQL Server Express, Developer или LocalDB
- NuGet restore для всех пакетов решения

### Клонирование репозитория

```bash
git clone <ссылка-на-ваш-репозиторий>
cd StockKeeperMail
```

### Запуск решения

1. Открой решение в Visual Studio
2. Дождись восстановления NuGet-пакетов
3. Убедись, что стартовым проектом выбран **StockKeeperMail.Desktop**
4. Собери решение в режиме **Debug** или **Release**

> [!IMPORTANT]
> Перед первым запуском обязательно настрой подключение к базе данных и примени миграции. Без этого приложение откроется, но рабочие сценарии, завязанные на данные, корректно работать не будут

## Настройка базы данных

1. Открой файл `StockKeeperMail.Database/dbconfig.json`
2. Укажи строку подключения к SQL Server или LocalDB
3. Открой **Package Manager Console**
4. Выбери проект **StockKeeperMail.Database** как проект по умолчанию
5. Выполни команду:

```powershell
Update-Database
```

Если потребуется создать новую миграцию:

```powershell
Add-Migration MigrationName
Update-Database
```

### Пример строки подключения

```json
{
  "ConnectionStrings": {
    "DB": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=VLGMedBD;Integrated Security=True;Encrypt=False;"
  }
}
```

> [!WARNING]
> Для входа в систему в базе должны существовать хотя бы одна роль и хотя бы один сотрудник. Если база пустая, сначала создай стартовые записи в таблицах `Role` и `Staff`

## XML / Summary-документация

Если ты будешь хранить XML-документацию в отдельной папке репозитория, README уже подготовлен под такие ссылки:

- [Документация Desktop-модуля](docs/xml/StockKeeperMail.Desktop.xml)
- [Документация Database-модуля](docs/xml/StockKeeperMail.Database.xml)

> [!NOTE]
> В текущем шаблоне README путь для summary-документации задан как **`docs/xml/`**. Если у тебя XML-файлы будут лежать в другой папке, просто замени ссылки на актуальные

## Планы по развитию

- автоматическое создание стартовой административной учетной записи
- безопасное хранение паролей и их хэширование
- улучшение seed-данных для первого запуска
- экспорт отчетов и документов
- доработка фильтрации и поиска
- расширение аналитики на главной панели
- публикация полной XML-документации по классам и модулям

---

<div align="right">
  <a href="#top">Наверх ↑</a>
</div>
