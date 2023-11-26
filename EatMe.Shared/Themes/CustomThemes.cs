using MudBlazor;
using MudBlazor.Utilities;

namespace EatMe.Shared.Themes {
    public class CustomThemes {
        public static MudTheme DraculaTheme = new MudTheme() {
            // Pallete only Modifies Dark Theme
            PaletteDark = new PaletteDark() { // Based on Dracula https://draculatheme.com/contribute#color-palette
                Primary = new MudColor("#FF79C6"), // Pink
                Secondary = new MudColor("#BD93F9"), // Purple
                Tertiary = new MudColor("#8BE9FD"), // Cyan
                Info = new MudColor("#F1FA8C"), // Yellow
                Success = new MudColor("#50FA7B"), // Green
                Warning = new MudColor("#FFB86C"),
                Error = new MudColor("#FF5555"),
                Background = new MudColor("#282A36"),
                Surface = new MudColor("#44475A"),
                AppbarBackground = new MudColor("#BD93F9"),

                // TODO Flesh out theme
            },

        };

        public static MudTheme NordTheme = new MudTheme() {
            // Pallete only Modifies Light Theme
        };

        public static MudTheme CyberpunkTheme = new MudTheme() {
            // Pallete only Modifies Dark Theme
        };

        public static MudTheme SythwaveTheme = new MudTheme() {
            // Pallete only Modifies Dark Theme
        };
    }
}
