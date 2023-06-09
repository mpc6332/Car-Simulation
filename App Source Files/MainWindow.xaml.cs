using System;
using System.Media;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Car_Simulation;

/// <summary>
///     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
///     Written by: Matthew Covone
///     How to use the car simulation program:
///     ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/// 
///     {Starting up the car}
///     --------------------------------------------------------------------------------------------------------------------
///     [1.] By default, you are allowed to use the following controls as the initial state of the car is off: Driver Door,
///     Brake Pedal, Dome Light, Headlights, & Seat belt.
///     Regardless of whether the instrument panel is on or off, the headlight icon will appear when activated. You may
///     also hear a door chime if the driver door is open and the ignition state is "Off".
///     When it comes to the dome lights, the "Auto" option will activate it in its on and off states depending on if the
///     car door is open or not.
/// 
///     [2.] To start up the car, you first have to put a key into the car (Selecting the "Off" option). Then, you should be
///     able to select the "Accessory" and, if the brakes are pressed, "On" options.
/// 
///     [3.] In the "Accessory" option for the Ignition, you can turn on the radio (It is not in this project since it isn't
///     a requirement). Just like with the "Off" state, you can also move to the "On" state if the brakes are pressed down.
///     Unlike the "Off" option, you can't select the "No key" option as you would need to revert back to the "Off" state
///     to do that.
/// 
///     [4.] In the "On" option for the Ignition, you unlock the rest of the controls in your arsenal: Gear shift, turn
///     signals, driver window, and windshield wipers
///
///
/// 
/// 
///     {While the car is on}
///     --------------------------------------------------------------------------------------------------------------------
///     [1.] When shifting through the 4 different states of a gear shift, the user has to be pressing down on the brakes or
///     else the states will be disabled.
/// 
///     [2.] When activating certain controls (particularly the headlights, seat belt, and turn signals), their icons will
///     show up on the instrument panel itself. It is worth noting that these icons will, with the exception of the headlights, will disappear from the instrument
///     panel when the ignition state goes from "On" to "Off" even if their respective control states had not changed to different states that would have otherwise
///     disabled them.
///     
///     [3.] When it comes to the dome lights, the "Auto" option will activate it in its "on" and "off" states depending on if
///     the car door is open or not.
///     
/// 
///     
///
/// 
///     {When turning the car off}
///     --------------------------------------------------------------------------------------------------------------------
///     [1.] If you want to turn the car off, you have to make sure that the shift gear
///     state is set to "park". From there, you'll be able to transition ignition state from "On" to "Off". You CAN'T, however, go from "Off" to
///     "Accessory" as the "Accessory" option
///     can only be activated when the user is in the state of needing to turn on the car (you technically say that it can
///     only be activated through a one-way process).
///     
///     [2.] The instrument panel will revert to the state it was in before the car was turned on.
/// 
///     [3.] Some icons for some controls on the instrument panel will disappear (technically not really as they are covered up by the old instrument panel).
///     Any timers for any animations should also be stopped as well.
///
///     [4.] Remember that in the "Off" state that you may also hear a
///      door chime if the driver door is opened at the same time.
///
/// 
///     {Other}
///     --------------------------------------------------------------------------------------------------------------------
///     [1.] There are two other buttons to be aware about: the exit and reset buttons.
///     The exit button has the user exit out of the program, while the reset button
///     will reset all of the controls to itself default state.
/// 
/// </summary>
public partial class MainWindow : Window
{
    private DispatcherTimer turnSignalTimer;
    private int windshieldState;
    private DispatcherTimer windshieldWiperTimer;
    private MediaPlayer chime_player;
    private bool shouldLoop;


    public MainWindow()
    {
        InitializeComponent();

    }

    // When loading up the window, it renders the turn signal and windshield wiper timers to come into existence.
    private void wdw_Main_ContentRendered(object sender, EventArgs e)
    {
        turnSignalTimer_Setup();
        windshieldWiperTimer_Setup();
        chime_player = new MediaPlayer();
        chime_player.Open(new Uri("SoundAssets/NissanDoorChime.wav", UriKind.Relative));
    }

    // The turn signal timer set up.
    private void turnSignalTimer_Setup()
    {
        turnSignalTimer = new DispatcherTimer();
        turnSignalTimer.Tick += turnSignalTimer_Tick;
        turnSignalTimer.Interval = TimeSpan.FromMilliseconds(900);
    }

    // What actions happen within an interval pertaining to the turn signal timer.
    private void turnSignalTimer_Tick(object sender, EventArgs e)
    {
        // Right arrow selected.
        if (btn_TS_opt_Right.IsChecked == true)
        {
            // If the right arrow is hidden, show it.
            if (img_rightArrow.Visibility == Visibility.Hidden)
                img_rightArrow.Visibility = Visibility.Visible;
            // Vice versa.
            else if (img_rightArrow.Visibility == Visibility.Visible) img_rightArrow.Visibility = Visibility.Hidden;
        }
        // left arrow selected.
        else if (btn_TS_opt_Left.IsChecked == true)
        {
            // If the left arrow is hidden, show it.
            if (img_leftArrow.Visibility == Visibility.Hidden)
                img_leftArrow.Visibility = Visibility.Visible;
            // Vice versa.
            else if (img_leftArrow.Visibility == Visibility.Visible) img_leftArrow.Visibility = Visibility.Hidden;
        }
    }

    // The windshield wiper timer set up.
    private void windshieldWiperTimer_Setup()
    {
        windshieldWiperTimer = new DispatcherTimer();
        windshieldWiperTimer.Tick += windshieldWiperTimer_Tick;
        windshieldWiperTimer.Interval = TimeSpan.FromMilliseconds(600);
    }

    // What actions happen within an interval pertaining to the windshield wiper timer.
    private void windshieldWiperTimer_Tick(object sender, EventArgs e)
    {
        if (windshieldState == 0)
        {
            img_wiper.Source =
                new BitmapImage(
                    new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/WiperIconLeft.png"));
            windshieldState++;
        }
        else if (windshieldState == 1)
        {
            img_wiper.Source =
                new BitmapImage(
                    new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/WiperIconRight.png"));
            windshieldState--;
        }
    }

    // Exits out of the program.
    private void btn_Exit_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    // Resets the program back to its initial state
    private void btn_reset_Click(object sender, RoutedEventArgs e)
    {
        btn_I_opt_NoKey.IsChecked = true;
        btn_BP_opt_Released.IsChecked = true;
        btn_GS_opt_Park.IsChecked = true;
        btn_TS_opt_Off.IsChecked = true;
        btn_DW_opt_Up.IsChecked = true;
        btn_WW_opt_Off.IsChecked = true;
        btn_SB_opt_Unbuckled.IsChecked = true;
        btn_DD_opt_Closed.IsChecked = true;
        btn_DLS_opt_Off.IsChecked = true;
        btn_DS_opt_Unoccupied.IsChecked = true;
        btn_HLS_opt_Off.IsChecked = true;
        instrumentLookUp(sender, e);


    }

    // Effects of the "Released" option for the car brakes.
    private void btn_BP_opt_Released_Checked(object sender, RoutedEventArgs e)
    {
        // Avoids a null exception upon initialization. Remember that the default option for the brakes upon starting up the executable is "Released".
        if (img_BrakeLights != null)
        {
            instrumentLookUp(sender, e);
            img_BrakeLights.Source =
                new BitmapImage(
                    new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/BrakeLightsOff.png"));
        }
    }

    // Effects of the "Pressed" option for the car brakes.
    private void btn_BP_opt_Pressed_Checked(object sender, RoutedEventArgs e)
    {
        instrumentLookUp(sender, e);
        img_BrakeLights.Source =
            new BitmapImage(new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/BrakeLightsOn.png"));
    }

    // Effects of the "No key" option for the car keyhole.
    private void btn_I_opt_NoKey_Checked(object sender, RoutedEventArgs e)
    {
        // Avoids a null exception upon initialization. Remember that the default option for the key hole upon starting up the executable is "No key". 
        if (img_KeyHole != null)
        {
            instrumentLookUp(sender, e);
            img_KeyHole.Source =
                new BitmapImage(
                    new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/KeyholeNoKey.png"));
            img_Instrument_Panel_Off.Visibility = Visibility.Visible;
        }
    }

    // Effects of the "Off" option for the car keyhole.
    private void btn_I_opt_Off_Checked(object sender, RoutedEventArgs e)
    {
        instrumentLookUp(sender, e);
        img_KeyHole.Source =
            new BitmapImage(new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/KeyholeOff.png"));
        img_Instrument_Panel_Off.Visibility = Visibility.Visible;
    }

    // Effects of the "Accessory" option for the car keyhole.
    private void btn_I_opt_Accessory_Checked(object sender, RoutedEventArgs e)
    {
        instrumentLookUp(sender, e);
        img_KeyHole.Source =
            new BitmapImage(
                new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/KeyholeAccessory.png"));
        img_Instrument_Panel_Off.Visibility = Visibility.Visible;
    }

    // Effects of the "On" option for the car keyhole. Notably, the instrument panel lights up when selecting this option.
    private void btn_I_opt_On_Checked(object sender, RoutedEventArgs e)
    {
        instrumentLookUp(sender, e);
        img_KeyHole.Source =
            new BitmapImage(new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/KeyholeOn.png"));
        img_Instrument_Panel_Off.Visibility = Visibility.Hidden;
        img_Instrument_Panel_On.Visibility = Visibility.Visible;
    }

    // Effects of the "Park" option for the gear shift. After pressing down on the brakes and selecting this option, the state of the brakes will revert back to "Pressed".
    private void btn_GS_opt_Park_Checked(object sender, RoutedEventArgs e)
    {
        // Avoids a null exception upon initialization. Remember that the default option for the gear shift upon starting up the executable is "Park". 
        if (img_DriveStick != null)
        {
            btn_BP_opt_Released.IsChecked = true;
            btn_I_opt_On.IsEnabled = true;
            img_DriveStick.Source =
                new BitmapImage(
                    new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/DriverStickP.jpg"));
        }
    }

    // Effects of the "Reverse" option for the gear shift. After pressing down on the brakes and selecting this option, the state of the brakes will revert back to "Pressed".
    private void btn_GS_opt_Reverse_Checked(object sender, RoutedEventArgs e)
    {
        btn_BP_opt_Released.IsChecked = true;
        btn_I_opt_On.IsEnabled = false;
        img_DriveStick.Source =
            new BitmapImage(new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/DriverStickR.jpg"));
    }

    // Effects of the "Neutral" option for the gear shift. After pressing down on the brakes and selecting this option, the state of the brakes will revert back to "Pressed".
    private void btn_GS_opt_Neutral_Checked(object sender, RoutedEventArgs e)
    {
        btn_BP_opt_Released.IsChecked = true;
        btn_I_opt_On.IsEnabled = false;
        img_DriveStick.Source =
            new BitmapImage(new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/DriverStickN.jpg"));
    }

    // Effects of the "Drive" option for the gear shift. After pressing down on the brakes and selecting this option, the state of the brakes will revert back to "Pressed".
    private void btn_GS_opt_Drive_Checked(object sender, RoutedEventArgs e)
    {
        btn_BP_opt_Released.IsChecked = true;
        btn_I_opt_On.IsEnabled = false;
        img_DriveStick.Source =
            new BitmapImage(new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/DriverStickD.jpg"));
    }

    // Effects of the "Off" option for the turn signals.
    private void btn_TS_opt_Off_Checked(object sender, RoutedEventArgs e)
    {
        // Avoids a null exception upon initialization. Remember that the default option for the turn signals upon starting up the executable is "Off".
        if ((img_leftArrow != null) & (img_rightArrow != null)) turnSignalTimerReset(sender, e);
    }

    // Effects of the "Left" option for the turn signals. 
    private void btn_TS_opt_Left_Checked(object sender, RoutedEventArgs e)
    {
        turnSignalTimerReset(sender, e);
        turnSignalTimer.Start();
    }

    // Effects of the "Right" option for the turn signals. 
    private void btn_TS_opt_Right_Checked(object sender, RoutedEventArgs e)
    {
        turnSignalTimerReset(sender, e);
        turnSignalTimer.Start();
    }

    // Effects of the "Up" option for the driver window. 
    private void btn_DW_opt_Up_Checked(object sender, RoutedEventArgs e)
    {
        // Avoids a null exception upon initialization. Remember that the default option for the driver window upon starting up the executable is "Up".
        if (img_DriverWindow != null)
            img_DriverWindow.Source =
                new BitmapImage(
                    new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/DoorWindowUp.png"));
    }

    // Effects of the "Down" option for the driver window.
    private void btn_DW_opt_Down_Checked(object sender, RoutedEventArgs e)
    {
        img_DriverWindow.Source =
            new BitmapImage(new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/DoorWindowDown.png"));
    }

    // Effects of the "Off" option for the windshield wiper.
    private void btn_WW_opt_Off_Checked(object sender, RoutedEventArgs e)
    {
        // Avoids a null exception upon initialization. Remember that the default option for the windshield upon starting up the executable is "Off".
        if (img_wiper != null)
        {
            windshieldWiperTimer.Stop();
            img_wiper.Source =
                new BitmapImage(
                    new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/WiperIconLeft.png"));
        }
    }

    // Effects of the "On" option for the windshield wiper.
    private void btn_WW_opt_On_Checked(object sender, RoutedEventArgs e)
    {
        windshieldWiperTimer.Start();
    }

    // Effects of the "Unbuckled"/unoccupied option for the driver seat belt.
    private void btn_SB_opt_Unbuckled_Checked(object sender, RoutedEventArgs e)
    {
        // Avoids a null exception upon initialization. Remember that the default option for the seat belt upon starting up the executable is "Unbuckled".
        if (img_seatbeltWarning != null) img_seatbeltWarning.Visibility = Visibility.Visible;
    }

    // Effects of the "Buckled"/occupied option for the driver seat belt.
    private void btn_SB_opt_Buckled_Checked(object sender, RoutedEventArgs e)
    {
        if (btn_I_opt_On.IsChecked == true) img_seatbeltWarning.Visibility = Visibility.Hidden;
    }

    // Effects of the "Closed" option for the driver door.
    private void btn_DD_opt_Closed_Checked(object sender, RoutedEventArgs e)
    {
        // Avoids a null exception upon initialization. Remember that the default option for the driver door upon starting up the executable is "Closed".
        if (img_doorOpen != null)
        {
            autoLightCheck(sender, e);
            img_doorOpen.Visibility = Visibility.Hidden;
        }
    }

    // Effects of the "Open" option for the driver door.
    private void btn_DD_opt_Open_Checked(object sender, RoutedEventArgs e)
    {
        autoLightCheck(sender, e);
        chimeCheck(sender, e);
        img_doorOpen.Visibility = Visibility.Visible;
    }

    // Effects of the "Off" option for the Dome Light Switch.
    private void btn_DLS_opt_Off_Checked(object sender, RoutedEventArgs e)
    {
        // Avoids a null exception upon initialization. Remember that the default option for the Dome Light Switch upon starting up the executable is "Off".
        if (img_LightDome != null)
            img_LightDome.Source =
                new BitmapImage(
                    new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/DomeLightOff.jpg"));
    }

    // Effects of the "On" option for the Dome Light Switch.
    private void btn_DLS_opt_On_Checked(object sender, RoutedEventArgs e)
    {
        img_LightDome.Source =
            new BitmapImage(new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/DomeLightOn.jpg"));
    }

    // Effects of the "Auto" option for the Dome Light Switch.
    private void btn_DLS_opt_Auto_Checked(object sender, RoutedEventArgs e)
    {
        autoLightCheck(sender, e);
    }

    // Effects of the "Unoccupied" option for the Driver seat.
    private void btn_DS_opt_Unoccupied_Checked(object sender, RoutedEventArgs e)
    {
        // Avoids a null exception upon initialization. Remember that the default option for the Driver upon starting up the executable is "Unoccupied".
        if (img_Driver != null)
            img_Driver.Source =
                new BitmapImage(
                    new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/DriverNotinSeat.png"));
    }

    // Effects of the "Occupied" option for the Driver seat.
    private void btn_DS_opt_Occupied_Checked(object sender, RoutedEventArgs e)
    {
        img_Driver.Source =
            new BitmapImage(
                new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/DriverinSeatIcon.png"));
    }

    // Effects of the "Off" option for the Headlights.
    private void btn_HLS_opt_Off_Checked(object sender, RoutedEventArgs e)
    {
        // Avoids a null exception upon initialization. Remember that the default option for the headlights upon starting up the executable is "Off".
        if (img_headlights != null) img_headlights.Visibility = Visibility.Hidden;
    }

    // Effects of the "On" option for the Headlights.
    private void btn_HLS_opt_On_Checked(object sender, RoutedEventArgs e)
    {
        img_headlights.Visibility = Visibility.Visible;
    }

    // Updates the status of each instrument option based primarily on the current ignition state and other conditions, such as pressed down brakes.
    public void instrumentLookUp(object sender, RoutedEventArgs e)
    {
        // Ignition state of "No key".
        if (btn_I_opt_NoKey.IsChecked == true)
        {
            sharedOptionDisabler(sender, e);
            btn_I_opt_On.IsEnabled = false;
            btn_I_opt_NoKey.IsEnabled = true;
            btn_I_opt_Accessory.IsEnabled = false;
        }

        // Ignition state of "Off".
        else if (btn_I_opt_Off.IsChecked == true)
        {
            sharedOptionDisabler(sender, e);
            btn_I_opt_Accessory.IsEnabled = true;
            btn_I_opt_NoKey.IsEnabled = true;
            chimeCheck(sender, e);

            // Will not enable the "On" option for the ignition unless the brakes are pressed.
            if (btn_BP_opt_Pressed.IsChecked == true)
                btn_I_opt_On.IsEnabled = true;
            else if (btn_BP_opt_Released.IsChecked == true) btn_I_opt_On.IsEnabled = false;

            turnSignalTimer.Stop();
            windshieldWiperTimer.Stop();
        }

        // Ignition state of "Accessory".
        else if (btn_I_opt_Accessory.IsChecked == true)
        {
            sharedOptionDisabler(sender, e);
            btn_I_opt_NoKey.IsEnabled = false;


            // Will not enable the "On" option for the ignition unless the brakes are pressed.
            if (btn_BP_opt_Pressed.IsChecked == true)
            {
                btn_I_opt_On.IsEnabled = true;
                btn_I_opt_Off.IsEnabled = true;
            }
            else if (btn_BP_opt_Released.IsChecked == true)
            {
                btn_I_opt_On.IsEnabled = false;
            }
        }

        // Ignition state of "On"
        else if (btn_I_opt_On.IsChecked == true)
        {
            btn_GS_opt_Park.IsEnabled = true;
            btn_GS_opt_Drive.IsEnabled = true;
            btn_GS_opt_Reverse.IsEnabled = true;
            btn_GS_opt_Neutral.IsEnabled = true;
            btn_WW_opt_Off.IsEnabled = true;
            btn_WW_opt_On.IsEnabled = true;
            btn_DW_opt_Down.IsEnabled = true;
            btn_DW_opt_Up.IsEnabled = true;
            btn_TS_opt_Left.IsEnabled = true;
            btn_TS_opt_Off.IsEnabled = true;
            btn_TS_opt_Right.IsEnabled = true;
            btn_I_opt_On.IsEnabled = true;
            btn_I_opt_Accessory.IsEnabled = false;

            // Cannot turn the car off unless the gear shift state is set to "park". 
            if (btn_GS_opt_Park.IsChecked == true)
            {
                btn_GS_opt_Park.IsEnabled = true;
                btn_GS_opt_Drive.IsEnabled = true;
                btn_GS_opt_Reverse.IsEnabled = true;
                btn_GS_opt_Neutral.IsEnabled = true;
            }

            // The gear shift can't be activated unless the brakes are at the very least pressed down.
            if (btn_BP_opt_Pressed.IsChecked == true)
            {
                btn_GS_opt_Park.IsEnabled = true;
                btn_GS_opt_Drive.IsEnabled = true;
                btn_GS_opt_Reverse.IsEnabled = true;
                btn_GS_opt_Neutral.IsEnabled = true;
            }
            else if (btn_BP_opt_Released.IsChecked == true)
            {
                btn_GS_opt_Park.IsEnabled = false;
                btn_GS_opt_Drive.IsEnabled = false;
                btn_GS_opt_Reverse.IsEnabled = false;
                btn_GS_opt_Neutral.IsEnabled = false;
            }

            // If the seat belt state hadn't change up until the car started, make sure that the symbol for it is either visible or not on the lit-up instrument panel.
            if (btn_SB_opt_Unbuckled.IsChecked == true)
            {
                img_seatbeltWarning.Visibility = Visibility.Visible;
            }
            else if (btn_SB_opt_Buckled.IsChecked == true)
            {
                img_seatbeltWarning.Visibility = Visibility.Hidden;
            }

            // If the turn signals were left either on or off prior to reverting the car's state to off, then restarting the car will allow them to resume their "animation" when pressing the "On" button for the next time.
            if (btn_TS_opt_Left.IsChecked == true || btn_TS_opt_Right.IsChecked == true)
            {
                turnSignalTimer.Start();
            }

            // If the windshield wiper was left either on or off prior to reverting the car's state to off, then restarting the car will allow them to resume their "animation" when pressing the "On" button for the next time.
            if (btn_WW_opt_On.IsChecked == true)
            {
                windshieldWiperTimer.Start();
            }
        }
    }

    // Method that is used by three ignition states (No key, Off, & Accessory) as these controls are in the same state across the board.
    public void sharedOptionDisabler(object sender, RoutedEventArgs e)
    {
        btn_GS_opt_Park.IsEnabled = false;
        btn_GS_opt_Drive.IsEnabled = false;
        btn_GS_opt_Reverse.IsEnabled = false;
        btn_GS_opt_Neutral.IsEnabled = false;
        btn_WW_opt_Off.IsEnabled = false;
        btn_WW_opt_On.IsEnabled = false;
        btn_DW_opt_Down.IsEnabled = false;
        btn_DW_opt_Up.IsEnabled = false;
        btn_TS_opt_Left.IsEnabled = false;
        btn_TS_opt_Off.IsEnabled = false;
        btn_TS_opt_Right.IsEnabled = false;
    }

    // Method that resets the turn signal timer and the visibility of the turn arrows to their initial states so that there aren't any issues the next the timer is triggered to start. 
    public void turnSignalTimerReset(object sender, RoutedEventArgs e)
    {
        turnSignalTimer.Stop();
        img_leftArrow.Visibility = Visibility.Hidden;
        img_rightArrow.Visibility = Visibility.Hidden;
    }

    // Method that sets the version that the auto light will show based on if the door is closed or open.
    public void autoLightCheck(object sender, RoutedEventArgs e)
    {
        if (btn_DD_opt_Open.IsChecked == true && btn_DLS_opt_Auto.IsChecked == true)
            img_LightDome.Source =
                new BitmapImage(
                    new Uri("pack://application:,,,/Car_Simulation;component/ImageAssets/DomeLightAutoDoorOpen.jpg"));
        else if (btn_DD_opt_Closed.IsChecked == true && btn_DLS_opt_Auto.IsChecked == true)
            img_LightDome.Source =
                new BitmapImage(new Uri(
                    "pack://application:,,,/Car_Simulation;component/ImageAssets/DomeLightAutoDoorClosed.jpg"));
    }

    // Method that checks to see if a door chime can be initiated upon the door being opened while a key is still in the car (aka the Ignition state of "Off").
    public void chimeCheck(object sender, RoutedEventArgs e)
    {
        if (btn_DD_opt_Open.IsChecked == true && btn_I_opt_Off.IsChecked == true)
        {
            shouldLoop = true;
            chime_player.Position = TimeSpan.Zero;
            chime_player.MediaEnded += chime_player_MediaEnded;
            chime_player.Play();
        }
    }

    // Method that repeatedly loops the chime sound byte.
    private void chime_player_MediaEnded(object sender, EventArgs e)
    {
        if (shouldLoop)
        {
            chime_player.Position = TimeSpan.Zero;
            chime_player.Play();
        }
    }

    // Stops playing the sound and loop if the Driver's door is closed.
    private void btn_DD_opt_Open_Unchecked(object sender, RoutedEventArgs e)
    {
        shouldLoop = false;
        chime_player.Stop();
    }

    // Stops playing the sound and loop if the Ignition state is anything but off.
    private void btn_I_opt_Off_Unchecked(object sender, RoutedEventArgs e)
    {
        shouldLoop = false;
        chime_player.Stop();
    }
}