export function ProductList({ loading, items, onEdit, onDelete }) {
  if (loading) return <div className="muted">Carregando...</div>
  if (!items?.length) return <div className="muted">Nenhum produto cadastrado.</div>

  const handleDelete = (id, name) => {
    const ok = window.confirm(
      `Tem certeza que deseja excluir "${name}"?\nEssa ação não pode ser desfeita.`
    );
    if (ok) onDelete(id);
  };

  return (
    <div className="list">
      {items.map(p => (
        <div key={p.id} className="product">
          <div className="product-head">
            <div className="row">
              <span className="product-name">{p.name}</span>
              <span className="pill">{p.category}</span>
            </div>
            <div className="row">
              <button className="btn" onClick={() => onEdit(p)}>Editar</button>
              <button
                className="btn btn-danger"
                onClick={() => handleDelete(p.id, p.name)}
                title="Excluir produto"
              >
                Deletar
              </button>
            </div>
          </div>
          <div className="row muted">
            <span>Preço: <strong>R$ {Number(p.price).toFixed(2)}</strong></span>
            <span>Quantidade: <strong>{p.quantityInStock}</strong></span>
            <span>Disponível: <strong>{p.available ? 'Sim' : 'Não'}</strong></span>
          </div>
          <div className="muted">Criado em: {new Date(p.createdAt).toLocaleString()}</div>
        </div>
      ))}
    </div>
  )
}
