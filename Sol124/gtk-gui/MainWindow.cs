
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.VBox vbox1;

	private global::Gtk.HBox hbox4;

	private global::Gtk.FileChooserButton filechooserbutton;

	private global::Gtk.Fixed fixed1;

	private global::Gtk.Label label1;

	private global::Gtk.Label label;

	private global::Gtk.HBox hbox1;

	private global::Gtk.ScrolledWindow GtkScrolledWindow3;

	private global::Gtk.TextView textview;

	private global::Gtk.ScrolledWindow GtkScrolledWindow1;

	private global::Gtk.TreeView treeview;

	private global::Gtk.ScrolledWindow GtkScrolledWindow2;

	private global::Gtk.TreeView stTree;

	private global::Gtk.Button button;

	private global::Gtk.Frame frame;

	private global::Gtk.Alignment GtkAlignment;

	private global::Gtk.ScrolledWindow GtkScrolledWindow;

	private global::Gtk.TextView commandLine;

	private global::Gtk.Label GtkLabel2;

	protected virtual void Build()
	{
		global::Stetic.Gui.Initialize(this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.hbox4 = new global::Gtk.HBox();
		this.hbox4.Name = "hbox4";
		this.hbox4.Spacing = 6;
		// Container child hbox4.Gtk.Box+BoxChild
		this.filechooserbutton = new global::Gtk.FileChooserButton(global::Mono.Unix.Catalog.GetString("Select a File"), ((global::Gtk.FileChooserAction)(0)));
		this.filechooserbutton.Name = "filechooserbutton";
		this.hbox4.Add(this.filechooserbutton);
		global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.filechooserbutton]));
		w1.Position = 0;
		// Container child hbox4.Gtk.Box+BoxChild
		this.fixed1 = new global::Gtk.Fixed();
		this.fixed1.Name = "fixed1";
		this.fixed1.HasWindow = false;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.label1 = new global::Gtk.Label();
		this.label1.Name = "label1";
		this.label1.Xalign = 0F;
		this.label1.LabelProp = global::Mono.Unix.Catalog.GetString("Symbol Table");
		this.fixed1.Add(this.label1);
		global::Gtk.Fixed.FixedChild w2 = ((global::Gtk.Fixed.FixedChild)(this.fixed1[this.label1]));
		w2.X = 133;
		w2.Y = 9;
		// Container child fixed1.Gtk.Fixed+FixedChild
		this.label = new global::Gtk.Label();
		this.label.Name = "label";
		this.label.Xalign = 1F;
		this.label.LabelProp = global::Mono.Unix.Catalog.GetString("Lexeme");
		this.fixed1.Add(this.label);
		global::Gtk.Fixed.FixedChild w3 = ((global::Gtk.Fixed.FixedChild)(this.fixed1[this.label]));
		w3.X = 63;
		w3.Y = 7;
		this.hbox4.Add(this.fixed1);
		global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.fixed1]));
		w4.Position = 1;
		w4.Expand = false;
		w4.Fill = false;
		this.vbox1.Add(this.hbox4);
		global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.hbox4]));
		w5.Position = 0;
		w5.Expand = false;
		w5.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.hbox1 = new global::Gtk.HBox();
		this.hbox1.Name = "hbox1";
		this.hbox1.Spacing = 6;
		// Container child hbox1.Gtk.Box+BoxChild
		this.GtkScrolledWindow3 = new global::Gtk.ScrolledWindow();
		this.GtkScrolledWindow3.Name = "GtkScrolledWindow3";
		this.GtkScrolledWindow3.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow3.Gtk.Container+ContainerChild
		this.textview = new global::Gtk.TextView();
		this.textview.CanFocus = true;
		this.textview.Name = "textview";
		this.GtkScrolledWindow3.Add(this.textview);
		this.hbox1.Add(this.GtkScrolledWindow3);
		global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.GtkScrolledWindow3]));
		w7.Position = 0;
		// Container child hbox1.Gtk.Box+BoxChild
		this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow();
		this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
		this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
		this.treeview = new global::Gtk.TreeView();
		this.treeview.CanFocus = true;
		this.treeview.Name = "treeview";
		this.GtkScrolledWindow1.Add(this.treeview);
		this.hbox1.Add(this.GtkScrolledWindow1);
		global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.GtkScrolledWindow1]));
		w9.Position = 1;
		// Container child hbox1.Gtk.Box+BoxChild
		this.GtkScrolledWindow2 = new global::Gtk.ScrolledWindow();
		this.GtkScrolledWindow2.Name = "GtkScrolledWindow2";
		this.GtkScrolledWindow2.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow2.Gtk.Container+ContainerChild
		this.stTree = new global::Gtk.TreeView();
		this.stTree.CanFocus = true;
		this.stTree.Name = "stTree";
		this.GtkScrolledWindow2.Add(this.stTree);
		this.hbox1.Add(this.GtkScrolledWindow2);
		global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.GtkScrolledWindow2]));
		w11.Position = 2;
		this.vbox1.Add(this.hbox1);
		global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.hbox1]));
		w12.Position = 1;
		// Container child vbox1.Gtk.Box+BoxChild
		this.button = new global::Gtk.Button();
		this.button.CanFocus = true;
		this.button.Name = "button";
		this.button.UseUnderline = true;
		this.button.Label = global::Mono.Unix.Catalog.GetString("Run");
		this.vbox1.Add(this.button);
		global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.button]));
		w13.Position = 2;
		w13.Expand = false;
		w13.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.frame = new global::Gtk.Frame();
		this.frame.Name = "frame";
		this.frame.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame.Gtk.Container+ContainerChild
		this.GtkAlignment = new global::Gtk.Alignment(0F, 0F, 1F, 1F);
		this.GtkAlignment.Name = "GtkAlignment";
		this.GtkAlignment.LeftPadding = ((uint)(12));
		// Container child GtkAlignment.Gtk.Container+ContainerChild
		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow();
		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
		this.commandLine = new global::Gtk.TextView();
		this.commandLine.CanFocus = true;
		this.commandLine.Name = "commandLine";
		this.GtkScrolledWindow.Add(this.commandLine);
		this.GtkAlignment.Add(this.GtkScrolledWindow);
		this.frame.Add(this.GtkAlignment);
		this.GtkLabel2 = new global::Gtk.Label();
		this.GtkLabel2.Name = "GtkLabel2";
		this.GtkLabel2.LabelProp = global::Mono.Unix.Catalog.GetString("<b>Console</b>");
		this.GtkLabel2.UseMarkup = true;
		this.frame.LabelWidget = this.GtkLabel2;
		this.vbox1.Add(this.frame);
		global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.frame]));
		w17.Position = 3;
		this.Add(this.vbox1);
		if ((this.Child != null))
		{
			this.Child.ShowAll();
		}
		this.DefaultWidth = 1062;
		this.DefaultHeight = 577;
		this.Show();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler(this.OnDeleteEvent);
		this.filechooserbutton.FileActivated += new global::System.EventHandler(this.OnOpen);
		this.filechooserbutton.SelectionChanged += new global::System.EventHandler(this.OnOpen);
		this.button.Clicked += new global::System.EventHandler(this.Run);
	}
}