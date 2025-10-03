import { useEffect, useState } from 'react'
import { Header } from './components/Header.jsx'
import { ProductForm } from './components/ProductForm.jsx'
import { ProductList } from './components/ProductList.jsx'
import { EditModal } from './components/EditModal.jsx'
import { useProducts } from './hooks/useProducts.js'
import './styles/theme.css'

export default function App() {
  const [theme, setTheme] = useState(() => localStorage.getItem('theme') ?? 'light')
  useEffect(() => {
    document.documentElement.setAttribute('data-theme', theme)
    localStorage.setItem('theme', theme)
  }, [theme])
  const toggleTheme = () => setTheme(t => t === 'dark' ? 'light' : 'dark')

  const { loading, items, fetchAll, createProduct, updateProduct, deleteProduct } = useProducts()
  const [editing, setEditing] = useState(null)

  useEffect(() => { fetchAll() }, [fetchAll])

  return (
    <div className="container">
      <Header theme={theme} onToggleTheme={toggleTheme} />

      <div className="grid">
        <section className="card">
          <h2>Novo produto</h2>
          <p>Preencha os campos abaixo para cadastrar um novo item.</p>
          <ProductForm onCreate={async (dto) => { await createProduct(dto); await fetchAll() }} />
        </section>

        <section className="card">
          <h2>Produtos</h2>
          <p>Gerencie seus itens. Clique em <strong>Editar</strong> para alterar.</p>
          <ProductList
            loading={loading}
            items={items}
            onEdit={(p) => setEditing(p)}
            onDelete={async (id) => { await deleteProduct(id); await fetchAll() }}
          />
        </section>
      </div>

      <EditModal
        open={!!editing}
        product={editing}
        onClose={() => setEditing(null)}
        onSave={async (id, dto) => {
          await updateProduct(id, dto)
          setEditing(null)
          await fetchAll()
        }}
      />
    </div>
  )
}
