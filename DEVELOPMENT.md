# 🛠 Руководство разработчика виджетов для ChattyWidgets

Добро пожаловать в руководство по созданию виджетов для платформы **ChattyWidgets**! Здесь описано всё, что нужно для создания стильных, производительных и безопасных виджетов.

---

## 📑 Содержание
1. [Анатомия виджета](#-анатомия-виджета)
2. [Манифест виджета (`manifest.json`)](#-манифест-виджета-manifestjson)
3. [Стандарт пропорций и адаптивности (4:3)](#-стандарт-пропорций-и-адаптивности-43)
4. [Правила пространства имён и авторизации](#-правила-пространства-имён-и-авторизации)
5. [JavaScript Bridge API (`window.chatty`)](#-javascript-bridge-api-windowchatty)
6. [Стилизация и оформление (Glassmorphism)](#-стилизация-и-оформление-glassmorphism)
7. [Локальное тестирование и проверка](#-локальное-тестирование-и-проверка)
8. [Публикация в каталоге сообщества](#-публикация-в-каталоге-сообщества)

---

## 📦 Анатомия виджета

Виджет представляет собой обычную веб-страницу (HTML5, CSS3, JavaScript) или нативный XAML-компонент, упакованный в архив с расширением `.chw`.

Структура каталога виджета:
```text
widgets/com.github.<ваш_логин>.<имя_виджета>/
├── manifest.json       # Обязательно: конфигурация, размеры, метаданные
├── index.html          # Обязательно: точка входа веб-виджета
├── icon.png            # Обязательно: квадратная иконка (128x128 или 256x256 px)
├── style.css           # Ваши стили
└── script.js           # Ваша логика
```

---

## 📜 Манифест виджета (`manifest.json`)

Файл `manifest.json` описывает параметры окна, размеры и поведение виджета:

```json
{
  "id": "com.github.ivan.crypto-rates",
  "name": "Живые Котировки",
  "version": "1.0.0",
  "author": "@ivan",
  "description": "Мониторинг курсов Bitcoin, Ethereum и Gram в реальном времени.",
  "category": "Финансы",
  "type": "web",
  "entry": "index.html",
  "icon": "icon.png",
  "defaultWidth": 320,
  "defaultHeight": 240,
  "minWidth": 240,
  "minHeight": 180,
  "resizable": true,
  "defaultLayer": "desktop",
  "clickThrough": false,
  "permissions": ["network"],
  "menuItems": [
    { "id": "refresh-rates", "title": "Обновить котировки" },
    { "id": "toggle-currency", "title": "Сменить валюту (USD/RUB)" }
  ]
}
```

### Основные поля манифеста:
| Поле | Тип | Описание |
| :--- | :--- | :--- |
| `id` | `string` | Уникальный идентификатор виджета (см. правила ниже). |
| `name` | `string` | Отображаемое название виджета в магазине и окне. |
| `version` | `string` | Версия в формате SemVer (`1.0.0`). |
| `author` | `string` | Имя автора (автоматически проверяется и связывается с GitHub). |
| `type` | `string` | Тип виджета: `"web"` (HTML/JS) или `"native"` (XAML). |
| `entry` | `string` | Главный файл запуска (обычно `index.html` или `Widget.xaml`). |
| `defaultWidth` / `defaultHeight` | `number` | Начальные размеры окна в пикселях (**320 × 240** — единый стандарт пропорций 4:3). |
| `minWidth` / `minHeight` | `number` | Минимальные размеры окна (**240 × 180** — пропорция 4:3 для компактного режима 75%). |
| `defaultLayer` | `string` | Слой по умолчанию: `"desktop"` (на рабочем столе), `"normal"` (окно), `"top"` (поверх всех окон). |
| `clickThrough` | `boolean` | `true`, если клики должны проходить насквозь на рабочий стол. |
| `menuItems` | `array` | Пользовательские пункты контекстного меню ПКМ. |

---

## 📐 Стандарт пропорций и адаптивности (4:3)

Для безупречной эстетики рабочего стола платформа ChattyWidgets использует **единый стандарт пропорций 4:3**.

### Зачем нужна пропорциональность?
Пользователи часто выстраивают виджеты в **один аккуратный горизонтальный ряд** вдоль верхней или нижней границы экрана. Если виджеты имеют разную высоту или случайные пропорции (например, один 300×140, другой 280×220, третий 340×200), строка выглядит «лесенкой» и теряет аккуратный вид. 

Единый формат **320×240** гарантирует:
- Все карточки в одном ряду имеют строго одинаковую высоту и пропорции.
- Идеальное масштабирование через системные пресеты в меню ПКМ:
  - **75% (Компактный)**: `240 × 180 px`
  - **100% (Обычный)**: `320 × 240 px`
  - **125% (Увеличенный)**: `400 × 300 px`
  - **150% (Большой)**: `480 × 360 px`

### 💡 Правила адаптивной верстки (XAML и Web)

1. **Не используйте фиксированную высоту внутренних элементов**:
   - Избегайте жестких значений вроде `Height="240"` на внутренних контейнерах.
   - В XAML используйте сетки `Grid` с пропорциональными строками/колонками (`*` и `Auto`), `UniformGrid` или `DockPanel`.
   - В Web используйте Flexbox (`display: flex`) или CSS Grid с относительными единицами (`%`, `fr`, `clamp()`).

2. **Масштабирование круговых шкал и векторной графики (`Viewbox`)**:
   - Векторные элементы (круговые индикаторы CPU/RAM, аналоговые циферблаты, большие значки) оборачивайте в `<Viewbox Stretch="Uniform">`. Это гарантирует, что датчики плавно уменьшатся при переключении на режим 75% (`240×180`) и не обрежутся.

3. **Защита текста от переполнения**:
   - Длинные заголовки, названия песен и новости защищайте обрезкой с многоточием: `TextTrimming="CharacterEllipsis"` (в XAML) или `text-overflow: ellipsis; white-space: nowrap; overflow: hidden;` (в CSS).
   - Для бегущей строки используйте встроенное поведение: `controls:MarqueeBehavior.EnableMarquee="True"` (родительский контейнер обязан иметь `ClipToBounds="True"`).

4. **Идеальные скругления углов при уменьшении размера**:
   - Базовый радиус скругления внешней границы: `CornerRadius="20"`.
   - В WPF `Border.ClipToBounds="True"` не скругляет растровые изображения внутри контейнера. При использовании `Grid.OpacityMask` задавайте `Stretch="None" AlignmentX="Left" AlignmentY="Top"` с привязкой через `ElementName` и внутренним радиусом `CornerRadius="19"`:
     ```xaml
     <Grid x:Name="MediaGrid">
         <Grid.OpacityMask>
             <VisualBrush Stretch="None" AlignmentX="Left" AlignmentY="Top">
                 <VisualBrush.Visual>
                     <Border Background="Black" CornerRadius="19"
                             Width="{Binding ActualWidth, ElementName=MediaGrid}"
                             Height="{Binding ActualHeight, ElementName=MediaGrid}"/>
                 </VisualBrush.Visual>
             </VisualBrush>
         </Grid.OpacityMask>
     ```
   - Любые пристыкованные снизу панели обязаны иметь нижний радиус скругления (например, `CornerRadius="0,0,19,19"`), иначе их прямые углы будут вылезать за пределы рамки при динамическом сжатии.

---

## 🔒 Правила пространства имён и авторизации

Чтобы никто не мог выдать свой виджет за официальный или подделать имя другого автора, в платформе действуют строгие правила:

1. **Официальные виджеты платформы**:
   - Префикс `com.chatty.*` зарезервирован исключительно за разработчиками ChattyWidgets (`chattytoaster`).
   - Официальные виджеты подписываются закрытым криптографическим RSA-ключом.

2. **Виджеты сообщества**:
   - Идентификатор `id` **обязан** начинаться с вашего реального логина на GitHub:
     - `com.github.<ваш_логин>.<имя_виджета>` или `<ваш_логин>.<имя_виджета>`.
   - При создании Pull Request робот сверяет ID с вашим авторизованным GitHub-аккаунтом. Если они не совпадают, PR отклоняется автоматически.
   - Поле `author` при сборке каталога принудительно заменяется на `@<ваш_логин>` со ссылкой на ваш GitHub.

---

## ⚡ JavaScript Bridge API (`window.chatty`)

В каждый веб-виджет среда ChattyWidgets автоматически внедряет объект `window.chatty`.

### 1. Мониторинг системы (Hardware API)
```javascript
// Процессор и память
const cpu = window.chatty.getCpu();             // % загрузки CPU (например, 24.5)
const ramPercent = window.chatty.getMemoryPercent(); // % загрузки RAM
const ramUsed = window.chatty.getMemoryUsed();       // Использовано RAM в ГБ
const ramTotal = window.chatty.getMemoryTotal();     // Всего RAM в ГБ

// Накопитель и сеть
const diskPercent = window.chatty.getDiskPercent();  // % использования диска
const diskFreeGb = window.chatty.getDiskFree();      // Свободно на диске в ГБ
const netDown = window.chatty.getNetDownload();      // Скорость загрузки (КБ/с)
const netUp = window.chatty.getNetUpload();          // Скорость отдачи (КБ/с)

// Получить все метрики одним JSON-объектом
const metrics = JSON.parse(window.chatty.getSystemMetricsJson());
```

### 2. Мультимедиа (GSMTC Media API)
Управление воспроизведением треков (Spotify, Яндекс.Музыка, браузеры, системные плееры):
```javascript
window.chatty.playPause();  // Пауза / Воспроизведение
window.chatty.nextTrack();  // Следующий трек
window.chatty.prevTrack();  // Предыдущий трек

// Получение информации о текущем треке
const media = JSON.parse(window.chatty.getMediaJson());
console.log(media.Title, media.Artist, media.IsPlaying);
```

### 3. Постоянное хранилище (Storage API)
Данные в хранилище сохраняются навсегда между перезапусками приложения:
```javascript
// Сохранить значение
window.chatty.storageSet('myKey', 'Привет, мир!');

// Прочитать значение (с дефолтным fallback)
const value = window.chatty.storageGet('myKey', 'По умолчанию');

// Удалить значение
window.chatty.storageRemove('myKey');
```

### 4. Управление окном виджета
```javascript
window.chatty.showContextMenu();   // Программно открыть контекстное меню виджета
window.chatty.closeWidget();        // Закрыть виджет
window.chatty.resize(350, 250);     // Изменить размер виджета
window.chatty.setLocked(true);      // Зафиксировать позицию
window.chatty.setClickThrough(true);// Включить режим "Клик насквозь"
window.chatty.setAutoHideDot(true); // Спрятать точку управления (показывать только при hover)
```

### 5. Обработка пунктов контекстного меню
Когда пользователь выбирает ваш кастомный пункт меню (из `menuItems` в манифесте), в окне вызывается событие:
```javascript
window.addEventListener('chatty:menu', function(e) {
    const menuId = e.detail.id;
    if (menuId === 'refresh-rates') {
        fetchRates();
    }
});
```

---

## 🎨 Стилизация и оформление (Glassmorphism)

Приложение ChattyWidgets оформлено в стиле **темного матового стекла**. Рекомендуется придерживаться общего визуального стиля:

- **Фон карточки**: полупрозрачный темный фон с размытием:
  ```css
  background: rgba(18, 22, 34, 0.75);
  backdrop-filter: blur(20px);
  border: 1px solid rgba(255, 255, 255, 0.08);
  border-radius: 18px;
  ```
- **Шрифты**: современный системный стек sans-serif:
  ```css
  font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
  color: #F8FAFC;
  ```
- **Точка управления**:
  В правом верхнем углу (12 px от краев) располагается синяя точка управления. Не размещайте важные интерактивные кнопки впритык к правому верхнему углу (оставьте отступ 30-36 px).

---

## 🧪 Локальное тестирование и проверка

Перед отправкой виджета в репозиторий протестируйте его встроенными инструментами:

```bash
# 1. Проверка структуры манифеста, безопасности и лимитов
python scripts/validate_widgets.py

# 2. Локальная сборка .chw пакета и проверка catalog.json
python scripts/build_catalog.py
```

Также в самом приложении **ChattyWidgets** есть встроенная **«Мастерская Разработчика»**:
1. Откройте приложение ChattyWidgets.
2. Перейдите во вкладку **«Мастерская»**.
3. Нажмите **«Создать шаблон виджета»** или **«Собрать .chw»** для проверки вашего виджета на живом рабочем столе.

---

## 🚀 Публикация в каталоге сообщества

1. Сделайте **Fork** репозитория [`ChattyWidgetsLibrary`](https://github.com/chattytoaster/ChattyWidgetsLibrary).
2. Поместите папку с вашим виджетом в `widgets/com.github.<ваш_логин>.<имя_виджета>/`.
3. Создайте **Pull Request**.
4. GitHub Actions автоматически проверит структуру и безопасность виджета.
5. После одобрения модератором виджет будет собран и появится в Магазине ChattyWidgets у всех пользователей мира!
