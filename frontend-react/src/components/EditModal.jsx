import { useEffect, useState } from 'react'

export function EditModal({ open, product, onClose, onSave }) {
  const [name, setName] = useState('')
  const [category, setCategory] = useState('')
  const [price, setPrice] = useState('')
  const [quantity, setQuantity] = useState('')
  const [createdAt, setCreatedAt] = useState('')

  useEffect(() => {
    if (product) {
      setName(product.name ?? '')
      setCategory(product.category ?? '')
      setPrice(product.price ?? '')
      setQuantity(product.quantityInStock ?? '')
      const dt = new Date(product.createdAt)
      setCreatedAt(`${dt.getFullYear()}-${String(dt.getMonth()+1).padStart(2,'0')}-${String(dt.getDate()).padStart(2,'0')}T${String(dt.getHours()).padStart(2,'0')}:${String(dt.getMinutes()).padStart(2,'0')}`)
    }
  }, [product])

  if (!open) return null

  const save = async () => {
    const dto = {
      name: name.trim(),
      category: category.trim(),
      price: parseFloat(price || 0),
      quantityInStock: parseInt(quantity || 0),
      createdAt: createdAt ? new Date(createdAt).toISOString() : new Date().toISOString()
    }
    await onSave(product.id, dto)
  }

  return (
    <div className="modal-backdrop" onClick={(e)=>{ if(e.target===e.currentTarget) onClose() }}>
      <div className="modal">
        <div className="modal-head">
          <strong>Editar produto</strong>
          <span className="close-x" title="Fechar" onClick={onClose}>&times;</span>
        </div>
        <div className="field">
          <label className="label">Nome</label>
          <input className="input" value={name} onChange={e=>setName(e.target.value)} />
        </div>
        <div className="field">
          <label className="label">Categoria</label>
          <input className="input" value={category} onChange={e=>setCategory(e.target.value)} />
        </div>
        <div className="row">
          <div className="field" style={{flex:'1 1 180px'}}>
            <label className="label">Preço</label>
            <input className="input" type="number" step="0.01" value={price} onChange={e=>setPrice(e.target.value)} />
          </div>
          <div className="field" style={{flex:'1 1 180px'}}>
            <label className="label">Quantidade</label>
            <input className="input" type="number" value={quantity} onChange={e=>setQuantity(e.target.value)} />
          </div>
        </div>
        <div className="field">
          <label className="label">Data de inclusão</label>
          <input className="input" type="datetime-local" value={createdAt} onChange={e=>setCreatedAt(e.target.value)} />
        </div>
        <div className="actions" style={{justifyContent:'flex-end'}}>
          <button className="btn btn-primary" type="button" onClick={save}>Salvar alterações</button>
        </div>
      </div>
    </div>
  )
}
