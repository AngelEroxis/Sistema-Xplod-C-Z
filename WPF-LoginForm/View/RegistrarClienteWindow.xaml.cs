using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_LoginForm.Model;

namespace WPF_LoginForm.View
{
    /// <summary>
    /// Lógica de interacción para RegistrarClienteWindow.xaml
    /// </summary>
    public partial class RegistrarClienteWindow : Window
    {
        public RegistrarClienteWindow()
        {
            InitializeComponent();
        }
        private Cliente clienteExistente;

        public RegistrarClienteWindow(Cliente cliente)
        {
            InitializeComponent();
            clienteExistente = cliente;

            // Rellenar los campos
            txtNombre.Text = cliente.Nombre;
            txtCI.Text = cliente.CI;
            txtNumItem.Text = cliente.NumItem;
            txtDomicilio.Text = cliente.Domicilio;
            txtCelular.Text = cliente.Celular;
            txtEmpresa.Text = cliente.EmpresaInstitucion;
            txtGarante.Text = cliente.Garante;
            txtCelGarante.Text = cliente.CelGarante;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtCI.Text) ||
                string.IsNullOrWhiteSpace(txtNumItem.Text) ||
                string.IsNullOrWhiteSpace(txtDomicilio.Text) ||
                string.IsNullOrWhiteSpace(txtCelular.Text) ||
                string.IsNullOrWhiteSpace(txtEmpresa.Text) ||
                string.IsNullOrWhiteSpace(txtGarante.Text) ||
                string.IsNullOrWhiteSpace(txtCelGarante.Text))
            {
                MessageBox.Show("Por favor completa todos los campos.", "Campos obligatorios", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (clienteExistente != null)
            {
                using (var context = new MyDbContext())
                {
                    var clienteDb = context.Clientes.FirstOrDefault(c => c.IdCliente == clienteExistente.IdCliente);
                    if (clienteDb != null)
                    {
                        clienteDb.Nombre = txtNombre.Text;
                        clienteDb.CI = txtCI.Text;
                        clienteDb.NumItem = txtNumItem.Text;
                        clienteDb.Domicilio = txtDomicilio.Text;
                        clienteDb.Celular = txtCelular.Text;
                        clienteDb.EmpresaInstitucion = txtEmpresa.Text;
                        clienteDb.Garante = txtGarante.Text;
                        clienteDb.CelGarante = txtCelGarante.Text;

                        context.SaveChanges();
                        MessageBox.Show("Cliente actualizado exitosamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.DialogResult = true;
                        this.Close();
                    }
                }
            }
            else
            {
                try
                {
                    using (var context = new MyDbContext())
                    {
                        var nuevoCliente = new Cliente
                        {
                            Nombre = txtNombre.Text,
                            CI = txtCI.Text,
                            NumItem = txtNumItem.Text,
                            Domicilio = txtDomicilio.Text,
                            Celular = txtCelular.Text,
                            EmpresaInstitucion = txtEmpresa.Text,
                            Garante = txtGarante.Text,
                            CelGarante = txtCelGarante.Text
                        };

                        context.Clientes.Add(nuevoCliente);
                        context.SaveChanges();
                        MessageBox.Show("Cliente registrado exitosamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.DialogResult = true;
                        this.Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Ocurrió un error al guardar el cliente.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
