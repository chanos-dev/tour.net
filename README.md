# tour.net
A simple library to easily implement tutorial functionality in WinForms.

Developed using .NET Framework 4.8.

## Preview
![sample](https://github.com/chanos-dev/tour.net/blob/main/sample.gif?raw=true)

## Usage
```csharp
public partial class MainForm : Form
{
    private ITutorial _tutorial;

    public MainForm()
    {
        InitializeComponent();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        InitializeTutorial();
    }

    private void InitializeTutorial()
    {
        var tutorial = new Tutorial(this)
            .SetTutorialConfig(config =>
            {
                config.TooltipColor = Color.LightBlue;
            })
            .AddStep(new HighlightForm(button1), new DefaultTooltipForm("Step 1", "click the button1.", ETooltipPosition.Right))
            .AddStep(new HighlightForm(button2), new DefaultTooltipForm("Step 2", "click the button2.", ETooltipPosition.Right))
            .AddStep(new HighlightForm(checkBox1), new DefaultTooltipForm("Step 3", "check the checkBox1.", ETooltipPosition.Top))
            .AddStep(new HighlightForm(radioButton1), new DefaultTooltipForm("Step 4", "click the radioButton1.", ETooltipPosition.Bottom))
            .AddStep(new HighlightForm(radioButton2), new DefaultTooltipForm("Step 5", "click the radioButton2.", ETooltipPosition.Left))
            .AddStep(new HighlightForm(radioButton3), new DefaultTooltipForm("Step 6", "click the radioButton3.", ETooltipPosition.Right));

        _tutorial = tutorial.Build();
    }

    private void button3_Click(object sender, EventArgs e)
    {
        _tutorial.Start();
    }
}
```

- The constructor of Tutorial takes the Form where the tutorial will be executed as a parameter.
- You can add tutorial steps using the AddStep method.
  - HighlightForm: Specifies the Control to be highlighted.
  - DefaultTooltipForm: Provides a default Tooltip design where you can specify the title and content.
- Finally, call the Build method to create an ITutorial interface to start the tutorial.
- Start the tutorial using the Start method of the ITutorial interface.

> The SetTutorialConfig method works when using DefaultTooltipForm.

## Custom Tooltip
In addition to the provided DefaultTooltipForm, you can create a custom Tooltip design by implementing your own form.

```csharp
public class TooltipForm : Form
{
    public TooltipForm()
    {
        ShowInTaskbar = false;
    }

    /// <summary>
    /// Adds an event handler for the Exit button click event.
    /// </summary>
    /// <param name="exitEvent">The event handler to add.</param>
    public virtual void AddExitEvent(EventHandler exitEvent) { }

    /// <summary>
    /// Adds an event handler for the Next button click event.
    /// </summary>
    /// <param name="nextEvent">The event handler to add.</param>
    public virtual void AddNextEvent(EventHandler nextEvent) { }

    /// <summary>
    /// Adds an event handler for the Previous button click event.
    /// </summary>
    /// <param name="prevEvent">The event handler to add.</param>
    public virtual void AddPrevEvent(EventHandler prevEvent) { }

    /// <summary>
    /// Removes an event handler for the Exit button click event.
    /// </summary>
    /// <param name="exitEvent">The event handler to remove.</param>
    public virtual void RemoveExitEvent(EventHandler exitEvent) { }

    /// <summary>
    /// Removes an event handler for the Next button click event.
    /// </summary>
    /// <param name="nextEvent">The event handler to remove.</param>
    public virtual void RemoveNextEvent(EventHandler nextEvent) { }

    /// <summary>
    /// Removes an event handler for the Previous button click event.
    /// </summary>
    /// <param name="prevEvent">The event handler to remove.</param>
    public virtual void RemovePrevEvent(EventHandler prevEvent) { }

    /// <summary>
    /// Sets the step information for the tooltip, such as the current step index and the total number of steps.
    /// </summary>
    /// <param name="stepIndex">The current step index.</param>
    /// <param name="totalStepsCount">The total number of steps.</param>
    public virtual void SetStepInfo(int stepIndex, int totalStepsCount) { }

    /// <summary>
    /// Moves the tooltip to the specified location based on the provided bounds of the highlighted control.
    /// </summary>
    /// <param name="highlightControlBounds">The bounds of the control to highlight.</param>
    public virtual void MoveTooltip(Rectangle highlightControlBounds)
    {
        Location = new Point(highlightControlBounds.X, highlightControlBounds.Y);
    }
}
```
When designing a custom Tooltip, the following implementations are required:

- Inherit from the TooltipForm class.
- Use the [Add/Remove]NextEvent, [Add/Remove]PrevEvent, [Add/Remove]ExitEvent methods to connect button events.
- (Optional) Specify the current Tooltip step index using the SetStepInfo method.
- Implement the MoveTooltip method to set the Tooltip's location.
  - By default, the Tooltip is positioned at (0, 0) of the highlighted Control.

> To prevent the TooltipForm from closing, settings like FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; may be necessary.

## TODO
- Automatic Tooltip position adjustment (when the Tooltip goes outside the bounds of the Form).