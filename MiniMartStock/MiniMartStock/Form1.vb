Imports System.Data
Imports System.Data.SqlClient

Public Class minimart
    Dim conStr As String = "Server=(LocalDB)\MSSQLLocalDB;AttachDBFilename=|DataDirectory|\Minimart.mdf"
    Dim conn As New SqlConnection(conStr)
    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If cmbID.SelectedIndex = 0 Or txtName.Text = "" Or txtPrice.Text = "" Or txtUnit.Text = "" Then
            MessageBox.Show("กรุณาใส่ข้อมูลก่อนทำการเพิ่มข้อมูล", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop)
        Else
            conn.Open()
            Dim sql As String = "INSERT INTO products(productid,
                                                productname,
                                                unit,
                                                priceperunit)
                             values (@proid,@proname,@unit,@price)"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("proid", cmbID.Text)
            cmd.Parameters.AddWithValue("proname", txtName.Text)
            cmd.Parameters.AddWithValue("unit", txtUnit.Text)
            cmd.Parameters.AddWithValue("price", txtPrice.Text)
            If cmd.ExecuteNonQuery = 1 Then
                MessageBox.Show("เพิ่มข้อมูลเรียบร้อย", "Insert Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("ไม่สามารถเพิ่มข้อมูลได้", "Insert Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
            conn.Close()
            Resetdata()
        End If

    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        If cmbID.SelectedIndex = -1 Then
            MessageBox.Show("กรุณาเลือกสินค้าที่ต้องการแก้ไข", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop)
        Else
            conn.Open()
            Dim sql As String = "UPDATE products
                                SET productname = @proname,
                                    priceperunit = @price,
                                    unit = @unit
                                where productid = @proid"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("proname", txtName.Text)
            cmd.Parameters.AddWithValue("price", txtPrice.Text)
            cmd.Parameters.AddWithValue("unit", txtUnit.Text)
            cmd.Parameters.AddWithValue("proid", cmbID.Text)
            If cmd.ExecuteNonQuery = 1 Then
                MessageBox.Show("แก้ไขข้อมูลเรียบร้อย", "Update Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("ไม่สามารถแก้ไขข้อมูลได้", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
            conn.Close()

            Resetdata()

        End If
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        Resetdata()
    End Sub

    Private Sub Resetdata()
        txtName.Clear()
        txtPrice.Clear()
        txtUnit.Clear()
        cmbID.Items.Clear()
        Showdata()
    End Sub

    Private Sub gridData_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles gridData.CellContentClick

    End Sub

    Private Sub minimart_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Showdata()
    End Sub

    Private Sub Showdata()
        conn.Open()
        Dim sql As String = "SELECT * FROM products"
        Dim cmd As New SqlCommand(sql, conn)
        Dim adapter As New SqlDataAdapter(cmd)
        Dim data As New DataSet()
        adapter.Fill(data, "Product")

        gridData.DataSource = data.Tables("Product")

        For i = 0 To data.Tables("Product").Rows.Count - 1
            cmbID.Items.Add(data.Tables("Product").Rows(i)("productid"))
        Next

        conn.Close()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        cmbID.Items.Clear()
        Showdata()
    End Sub

    Private Sub cmbID_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbID.SelectedIndexChanged
        If cmbID.SelectedIndex <> -1 Then
            conn.Open()
            Dim sql As String = "SELECT * FROM products
                                 where productid = @proid"
            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("proid", cmbID.Text)

            Dim adapter As New SqlDataAdapter(cmd)
            Dim data As New DataSet()
            adapter.Fill(data, "Product")

            txtName.Text = data.Tables("Product").Rows(0)("productname")
            txtUnit.Text = data.Tables("Product").Rows(0)("Unit")
            txtPrice.Text = data.Tables("Product").Rows(0)("priceperunit")
            conn.Close()
        End If
    End Sub
End Class