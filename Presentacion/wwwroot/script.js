// script.js

/************************************************************
 * ESTADO DE LA APLICACIÓN
 ************************************************************/
const state = {
    token: localStorage.getItem('authToken') || null,
    isLoginMode: true,
    currentUser: null,
    currentGastoId: null, // Para edición
    currentCategoriaId: null // Para edición
};

/************************************************************
 * REFERENCIAS AL DOM (ELEMENTS)
 ************************************************************/
const elements = {
    // Layout Principal
    encabezadoPrincipal: document.getElementById('encabezadoPrincipal'),
    navegacionPrincipal: document.getElementById('navegacionPrincipal'),
    contenedorDashboard: document.getElementById('contenedorDashboard'),

    // Botones Header
    btnAbrirPerfil: document.getElementById('btnAbrirPerfil'),
    btnAbrirReportes: document.getElementById('btnAbrirReportes'),
    btnGestionarMetodos: document.getElementById('btnGestionarMetodos'),
    btnCerrarSesion: document.getElementById('btnCerrarSesion'),

    // Dashboard - Presupuesto
    medidorPresupuesto: document.getElementById('medidorPresupuesto'),
    textoPorcentajePresupuesto: document.getElementById('textoPorcentajePresupuesto'),
    mostrarTotalGastado: document.getElementById('mostrarTotalGastado'),
    mostrarPresupuestoTotal: document.getElementById('mostrarPresupuestoTotal'),
    estadoPresupuesto: document.getElementById('estadoPresupuesto'),

    // Dashboard - Gastos
    seccionGastos: document.getElementById('seccionGastos'),
    btnAgregarGasto: document.getElementById('btnAgregarGasto'),
    tablaGastos: document.getElementById('tablaGastos'),
    cuerpoTablaGastos: document.getElementById('cuerpoTablaGastos'),
    btnAbrirFiltros: document.getElementById('btnAbrirFiltros'),

    // Dashboard - Categorías
    seccionCategorias: document.getElementById('seccionCategorias'),
    btnAgregarCategoria: document.getElementById('btnAgregarCategoria'),
    contenedorListaCategorias: document.getElementById('contenedorListaCategorias'),

    // Modales
    modalDetalleGenerico: document.getElementById('modalDetalleGenerico'),
    modalFormularioGasto: document.getElementById('modalFormularioGasto'),
    modalPerfil: document.getElementById('modalPerfil'),
    modalReportes: document.getElementById('modalReportes'),
    modalMetodosPago: document.getElementById('modalMetodosPago'),
    modalFiltros: document.getElementById('modalFiltros'),

    // Forms y Inputs específicos
    formularioGasto: document.getElementById('formularioGasto'),
    gastoCategoriaSelect: document.getElementById('gastoCategoria'),
    gastoMetodoSelect: document.getElementById('gastoMetodo'),

    formularioPerfil: document.getElementById('formularioPerfil'),

    formularioReportes: document.getElementById('formularioReportes'),
    formularioImportar: document.getElementById('formularioImportar'),

    listaMetodosPago: document.getElementById('listaMetodosPago'),
    formularioMetodoPago: document.getElementById('formularioMetodoPago'),

    formularioFiltros: document.getElementById('formularioFiltros'),
    filtroCategoria: document.getElementById('filtroCategoria'),
    filtroMetodo: document.getElementById('filtroMetodo'),
    btnLimpiarFiltros: document.getElementById('btnLimpiarFiltros'),
    btnAplicarFiltros: document.getElementById('btnAplicarFiltros'),

    // Botones cierre modales
    botonesCerrarModal: document.querySelectorAll('.btnCerrarModal')
};

/************************************************************
 * DEFINICIÓN DE API
 ************************************************************/
const API = {
    auth: {
        login: '/api/auth/login',
        register: '/api/auth/register'
    },
    usuarios: {
        perfil: '/api/usuarios/perfil'
    },
    gastos: {
        base: '/api/gastos',
        usuario: '/api/gastos/usuario/', // GET lista
        buscar: '/api/gastos/buscar',   // POST filtro
        byId: (id) => `/api/gastos/${id}`
    },
    categorias: {
        base: '/api/categorias',
        buscar: '/api/categorias/buscar',
        byId: (id) => `/api/categorias/${id}`
    },
    presupuestos: {
        general: '/api/presupuestos/general',
        porcentaje: '/api/presupuestos/porcentaje-general',
        diferencia: '/api/presupuestos/diferencia-general',
        resumenCategorias: '/api/presupuestos/resumen-categorias'
    },
    metodosPago: {
        base: '/api/metodos-pago',
        byId: (id) => `/api/metodos-pago/${id}`
    },
    reportes: {
        mensual: '/api/reportes/mensual',
        exportar: (formato) => `/api/reportes/exportar?formato=${formato}`,
        importar: '/api/reportes/importar'
    }
};

/************************************************************
 * UTILIDADES
 ************************************************************/
function getAuthHeaders() {
    return {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${state.token}`
    };
}

function getFileHeaders() {
    return {
        'Authorization': `Bearer ${state.token}`
    };
}

const formatCurrency = (amount) => {
    return new Intl.NumberFormat('es-DO', { style: 'currency', currency: 'DOP' }).format(amount);
};

const formatDateInput = (dateString) => {
    if (!dateString) return '';
    // Toma la parte de la fecha YYYY-MM-DD
    return dateString.split('T')[0];
};

/************************************************************
 * INICIALIZACIÓN
 ************************************************************/
document.addEventListener('DOMContentLoaded', () => {
    injectLoginView(); // Crear HTML de login dinámicamente
    setupEventListeners();

    if (state.token) {
        initApp();
    } else {
        showLoginView();
    }
});

function initApp() {
    document.getElementById('contenedorLogin').classList.add('hidden');
    elements.encabezadoPrincipal.style.display = 'flex';
    elements.contenedorDashboard.style.display = 'flex';

    // Carga paralela de datos
    loadDashboardData();
}

async function loadDashboardData() {
    await Promise.all([
        loadPresupuestoInfo(),
        loadGastos(),
        loadCategoriasSideBar()
    ]);
}

/************************************************************
 * VISTAS Y AUTENTICACIÓN
 ************************************************************/
function injectLoginView() {
    const loginDiv = document.createElement('div');
    loginDiv.id = 'contenedorLogin';
    loginDiv.className = 'login-container'; // Asumimos estilos básicos en CSS o agregarlos
    loginDiv.innerHTML = `
        <div class="login-box" style="max-width:400px; margin: 100px auto; padding: 20px; border: 1px solid #ccc; border-radius: 8px; text-align: center;">
            <h2 id="loginTitle">Iniciar Sesión</h2>
            <form id="authForm">
                <div id="fieldNombre" style="display:none; margin-bottom: 10px;">
                    <input type="text" id="authNombre" placeholder="Nombre completo" style="width: 100%; padding: 8px;">
                </div>
                <div style="margin-bottom: 10px;">
                    <input type="email" id="authEmail" placeholder="Correo electrónico" required style="width: 100%; padding: 8px;">
                </div>
                <div style="margin-bottom: 10px;">
                    <input type="password" id="authPassword" placeholder="Contraseña" required style="width: 100%; padding: 8px;">
                </div>
                <button type="submit" id="btnAuthAction" style="width: 100%; padding: 10px; cursor: pointer;">Entrar</button>
            </form>
            <p style="margin-top: 15px;">
                <a href="#" id="toggleAuthMode">¿No tienes cuenta? Regístrate</a>
            </p>
        </div>
    `;
    document.body.appendChild(loginDiv);

    // Event Listeners del Login inyectado
    document.getElementById('authForm').addEventListener('submit', handleAuthSubmit);
    document.getElementById('toggleAuthMode').addEventListener('click', toggleAuthMode);
}

function showLoginView() {
    document.getElementById('contenedorLogin').classList.remove('hidden');
    elements.encabezadoPrincipal.style.display = 'none';
    elements.contenedorDashboard.style.display = 'none';
}

function toggleAuthMode(e) {
    e.preventDefault();
    state.isLoginMode = !state.isLoginMode;
    const title = document.getElementById('loginTitle');
    const btn = document.getElementById('btnAuthAction');
    const toggle = document.getElementById('toggleAuthMode');
    const fieldNombre = document.getElementById('fieldNombre');

    if (state.isLoginMode) {
        title.textContent = 'Iniciar Sesión';
        btn.textContent = 'Entrar';
        toggle.textContent = '¿No tienes cuenta? Regístrate';
        fieldNombre.style.display = 'none';
        document.getElementById('authNombre').required = false;
    } else {
        title.textContent = 'Registrarse';
        btn.textContent = 'Registrarse';
        toggle.textContent = '¿Ya tienes cuenta? Inicia Sesión';
        fieldNombre.style.display = 'block';
        document.getElementById('authNombre').required = true;
    }
}

async function handleAuthSubmit(e) {
    e.preventDefault();
    const email = document.getElementById('authEmail').value;
    const password = document.getElementById('authPassword').value;
    const nombre = document.getElementById('authNombre').value;

    const url = state.isLoginMode ? API.auth.login : API.auth.register;
    const body = state.isLoginMode
        ? { email, password }
        : { nombre, email, password };

    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body)
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText);
        }

        const data = await response.json();
        // Backend devuelve { token: "..." }
        state.token = data.token;
        localStorage.setItem('authToken', state.token);

        initApp();

    } catch (error) {
        alert('Error de autenticación: ' + error.message);
    }
}

function handleLogout() {
    localStorage.removeItem('authToken');
    state.token = null;
    state.currentUser = null;
    location.reload(); // Recargar para limpiar estados
}

/************************************************************
 * LÓGICA DE PRESUPUESTO
 ************************************************************/
async function loadPresupuestoInfo() {
    try {
        const [resGeneral, resPorcentaje, resDiferencia] = await Promise.all([
            fetch(API.presupuestos.general, { headers: getAuthHeaders() }),
            fetch(API.presupuestos.porcentaje, { headers: getAuthHeaders() }),
            fetch(API.presupuestos.diferencia, { headers: getAuthHeaders() })
        ]);

        if (resGeneral.ok && resPorcentaje.ok && resDiferencia.ok) {
            const presupuestoTotal = await resGeneral.json();
            const porcentaje = await resPorcentaje.json();
            const diferencia = await resDiferencia.json(); // Lo que queda (o negativo si excedió)

            const gastado = presupuestoTotal - diferencia;

            elements.mostrarPresupuestoTotal.textContent = formatCurrency(presupuestoTotal);
            elements.mostrarTotalGastado.textContent = formatCurrency(gastado);

            elements.medidorPresupuesto.value = porcentaje;
            elements.textoPorcentajePresupuesto.textContent = `${porcentaje}%`;

            if (diferencia >= 0) {
                elements.estadoPresupuesto.textContent = "Dentro del presupuesto";
                elements.estadoPresupuesto.style.color = "green";
            } else {
                elements.estadoPresupuesto.textContent = "Presupuesto Excedido";
                elements.estadoPresupuesto.style.color = "red";
            }
        }
    } catch (error) {
        console.error("Error cargando presupuesto", error);
    }
}

/************************************************************
 * GESTIÓN DE GASTOS
 ************************************************************/
async function loadGastos(filtro = null) {
    try {
        let url = API.gastos.usuario;
        let method = 'GET';
        let body = null;

        if (filtro) {
            url = API.gastos.buscar;
            method = 'POST';
            body = JSON.stringify(filtro);
        }

        const response = await fetch(url, {
            method: method,
            headers: getAuthHeaders(),
            body: body
        });

        if (response.ok) {
            const gastos = await response.json();
            renderTablaGastos(gastos);
        }
    } catch (error) {
        console.error("Error cargando gastos", error);
    }
}

function renderTablaGastos(gastos) {
    elements.cuerpoTablaGastos.innerHTML = '';

    if (gastos.length === 0) {
        elements.cuerpoTablaGastos.innerHTML = '<tr><td colspan="5" style="text-align:center;">No hay gastos registrados</td></tr>';
        return;
    }

    gastos.forEach(g => {
        // g es GastoVistaPrevia (Id, Encabezado, NombreCategoria, Monto)
        // Nota: GastoVistaPrevia no trae Fecha ni Metodo. Si necesitamos esos datos en tabla,
        // deberíamos ajustar el backend o hacer fetch individual (no recomendado por performance).
        // Asumiremos que para esta tabla simple usamos lo que hay o ajustamos la llamada.
        // *Corrección*: El backend GastoMapper.MapVistaPrevia no incluye fecha. 
        // Para mostrar fecha necesitamos GastoReadDTO o ajustar el VistaPrevia. 
        // Por ahora mostraremos "-" en fecha si no viene.

        const row = document.createElement('tr');
        row.innerHTML = `
            <td>Ver detalle</td> 
            <td>${g.nombreCategoria}</td>
            <td>${formatCurrency(g.monto)}</td>
            <td>-</td> 
            <td>
                <button class="btn-editar-gasto" data-id="${g.id}">Editar</button>
                <button class="btn-eliminar-gasto" data-id="${g.id}">Eliminar</button>
            </td>
        `;
        // Click en la fila (menos botones) para ver detalle
        // row.cells[0].onclick = () => showGastoDetail(g.id);

        // Listeners botones
        row.querySelector('.btn-editar-gasto').onclick = () => openModalGasto(g.id);
        row.querySelector('.btn-eliminar-gasto').onclick = () => deleteGasto(g.id);

        elements.cuerpoTablaGastos.appendChild(row);
    });
}

async function openModalGasto(id = null) {
    // 1. Cargar selects
    await populateSelects();

    state.currentGastoId = id;
    elements.formularioGasto.reset();

    if (id) {
        document.getElementById('tituloModalGasto').textContent = "Editar Gasto";
        // Fetch detalle
        try {
            const res = await fetch(API.gastos.byId(id), { headers: getAuthHeaders() });
            if (res.ok) {
                const gasto = await res.json();
                document.getElementById('gastoId').value = gasto.id;
                document.getElementById('gastoEncabezado').value = gasto.encabezado;
                document.getElementById('gastoMonto').value = gasto.monto;
                document.getElementById('gastoCategoria').value = buscarIdCategoriaPorNombre(gasto.nombreCategoria); // Problema: El ReadDTO devuelve Nombre, el CreateDTO pide ID.
                // *Solución*: Necesitamos mapear nombres a IDs o cambiar el backend. 
                // Asumiremos que el frontend guarda mapeo de categorias al cargarlas.
                // Por simplicidad, seleccionaremos por texto si es posible o el usuario reselecciona.

                // Intentar seleccionar por texto en el select
                setSelectByText(elements.gastoCategoriaSelect, gasto.nombreCategoria);
                setSelectByText(elements.gastoMetodoSelect, gasto.metodoDePago);

                document.getElementById('gastoFecha').value = formatDateInput(gasto.fecha);
                document.getElementById('gastoDescripcion').value = gasto.descripcion || '';
            }
        } catch (e) { alert("Error cargando gasto"); }
    } else {
        document.getElementById('tituloModalGasto').textContent = "Nuevo Gasto";
        document.getElementById('gastoFecha').valueAsDate = new Date();
    }

    elements.modalFormularioGasto.showModal();
}

// Helper para seleccionar option por texto (ya que el ReadDTO devuelve nombre)
function setSelectByText(select, text) {
    for (var i = 0; i < select.options.length; i++) {
        if (select.options[i].text === text) {
            select.selectedIndex = i;
            break;
        }
    }
}

async function handleGastoSubmit(e) {
    e.preventDefault();

    const id = document.getElementById('gastoId').value;
    const encabezado = document.getElementById('gastoEncabezado').value;
    const monto = parseFloat(document.getElementById('gastoMonto').value);
    const categoriaId = elements.gastoCategoriaSelect.value;
    const metodoId = elements.gastoMetodoSelect.value;
    const fecha = document.getElementById('gastoFecha').value;
    const descripcion = document.getElementById('gastoDescripcion').value;

    const dto = {
        id: id || null, // null si es nuevo
        encabezado,
        monto,
        categoriaId,
        metodoDePagoId: metodoId,
        descripcion,
        fecha: fecha ? fecha : null,
        isFechaActual: !fecha
    };

    try {
        let res;
        if (id) {
            // Update
            dto.id = id; // Asegurar GUID
            res = await fetch(API.gastos.byId(id), {
                method: 'PUT',
                headers: getAuthHeaders(),
                body: JSON.stringify(dto)
            });
        } else {
            // Create
            res = await fetch(API.gastos.base, {
                method: 'POST',
                headers: getAuthHeaders(),
                body: JSON.stringify(dto)
            });
        }

        if (res.ok || res.status === 201 || res.status === 204) {
            // Revisar alertas (solo en POST devuelve cuerpo con alertas, PUT es NoContent)
            if (res.status === 201) {
                const data = await res.json();
                if (data.alertas && data.alertas.length > 0) {
                    alert("Gasto guardado con alertas:\n" + data.alertas.join("\n"));
                } else {
                    alert("Gasto guardado exitosamente.");
                }
            } else {
                alert("Gasto actualizado.");
            }

            elements.modalFormularioGasto.close();
            // Recargar todo
            loadDashboardData();
        } else {
            const err = await res.text();
            alert("Error: " + err);
        }
    } catch (error) {
        console.error(error);
        alert("Error de conexión");
    }
}

async function deleteGasto(id) {
    if (!confirm("¿Eliminar este gasto?")) return;
    try {
        const res = await fetch(API.gastos.byId(id), { method: 'DELETE', headers: getAuthHeaders() });
        if (res.ok) loadDashboardData();
        else alert("No se pudo eliminar");
    } catch (e) { console.error(e); }
}

/************************************************************
 * GESTIÓN DE CATEGORÍAS
 ************************************************************/
async function loadCategoriasSideBar() {
    try {
        const res = await fetch(API.presupuestos.resumenCategorias, { headers: getAuthHeaders() });
        if (res.ok) {
            const categorias = await res.json();
            renderListaCategorias(categorias);
        }
    } catch (e) { console.error(e); }
}

function renderListaCategorias(categorias) {
    elements.contenedorListaCategorias.innerHTML = '';
    const ul = document.createElement('ul');
    ul.className = 'lista-categorias';

    categorias.forEach(c => {
        const li = document.createElement('li');
        li.className = 'item-categoria';
        if (c.isExcedido) li.style.color = 'red';

        li.innerHTML = `
            <span>${c.nombre} (${c.porcentajePresupuesto}%)</span>
            <small>${formatCurrency(c.montoPresupuesto)}</small>
        `;

        // Doble click para editar (opcional)
        li.ondblclick = () => openModalCategoria(c.id);

        ul.appendChild(li);
    });
    elements.contenedorListaCategorias.appendChild(ul);
}

// Nota: HTML no tiene form de categoria, usaremos prompt simple o inyectar form en modal generico
// Por simplicidad para este script, usaremos el botón para crear solo con Prompt,
// pero idealmente deberías crear un <dialog> específico en el HTML.
async function openModalCategoria() {
    // Ejemplo básico usando prompts ya que no hay modal específico en el HTML provisto
    // Si quisieras usar modalDetalleGenerico, habría que construir el form dentro con JS.
    const nombre = prompt("Nombre de la categoría:");
    if (!nombre) return;
    const presupuesto = prompt("Presupuesto límite:");
    if (!presupuesto) return;

    const dto = {
        nombre: nombre,
        montoPresupuesto: parseFloat(presupuesto),
        usuarioId: state.currentUser?.id || '00000000-0000-0000-0000-000000000000' // El backend lo sobreescribe con el token
    };

    try {
        const res = await fetch(API.categorias.base, {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify(dto)
        });
        if (res.ok) {
            alert("Categoría creada");
            loadDashboardData();
        } else {
            alert("Error creando categoría");
        }
    } catch (e) { console.error(e); }
}


/************************************************************
 * GESTIÓN DE MÉTODOS DE PAGO Y FILTROS
 ************************************************************/
async function populateSelects() {
    // Categorias
    try {
        const resCat = await fetch(API.categorias.base, { headers: getAuthHeaders() });
        const categorias = await resCat.json();

        const resMet = await fetch(API.metodosPago.base, { headers: getAuthHeaders() });
        const metodos = await resMet.json();

        // Llenar selects de Gasto Modal
        fillSelect(elements.gastoCategoriaSelect, categorias, 'id', 'nombre');
        fillSelect(elements.gastoMetodoSelect, metodos, 'id', 'nombre');

        // Llenar selects de Filtros Modal
        fillSelect(elements.filtroCategoria, categorias, 'id', 'nombre', true);
        fillSelect(elements.filtroMetodo, metodos, 'id', 'nombre', true);

    } catch (e) { console.error("Error llenando selects", e); }
}

function fillSelect(selectElement, items, valueKey, textKey, addDefault = false) {
    selectElement.innerHTML = '';
    if (addDefault) {
        const opt = document.createElement('option');
        opt.value = '';
        opt.text = 'Todos / Seleccione';
        selectElement.appendChild(opt);
    }
    items.forEach(item => {
        const opt = document.createElement('option');
        opt.value = item[valueKey];
        opt.text = item[textKey];
        selectElement.appendChild(opt);
    });
}


/************************************************************
 * REPORTES
 ************************************************************/
async function handleExportarReporte(e) {
    e.preventDefault();
    const formato = document.getElementById('formatoReporte').value;
    try {
        const res = await fetch(API.reportes.exportar(formato), { headers: getAuthHeaders() });
        if (res.ok) {
            const blob = await res.blob();
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            const ext = formato === 'Excel' ? 'xlsx' : (formato === 'Json' ? 'json' : 'txt');
            a.download = `Reporte_${new Date().toISOString().split('T')[0]}.${ext}`;
            document.body.appendChild(a);
            a.click();
            a.remove();
        } else {
            alert("Error al exportar");
        }
    } catch (err) { alert(err.message); }
}

async function handleImportarReporte(e) {
    e.preventDefault();
    const fileInput = document.getElementById('archivoImportar');
    if (fileInput.files.length === 0) return alert("Seleccione un archivo");

    const formData = new FormData();
    formData.append('archivo', fileInput.files[0]);

    try {
        const res = await fetch(API.reportes.importar, {
            method: 'POST',
            headers: getFileHeaders(), // Solo Authorization, NO Content-Type (el navegador lo pone con boundary)
            body: formData
        });
        if (res.ok) {
            alert("Importación exitosa");
            elements.modalReportes.close();
            loadDashboardData();
        } else {
            alert("Error en la importación");
        }
    } catch (err) { alert(err.message); }
}


/************************************************************
 * MÉTODOS DE PAGO
 ************************************************************/
async function openModalMetodos() {
    await loadMetodosPagoList();
    elements.modalMetodosPago.showModal();
}

async function loadMetodosPagoList() {
    const res = await fetch(API.metodosPago.base, { headers: getAuthHeaders() });
    const metodos = await res.json();
    elements.listaMetodosPago.innerHTML = '';

    metodos.forEach(m => {
        const li = document.createElement('li');
        li.innerHTML = `
            <strong>${m.nombre}</strong> (${m.tipoPago})
            <button onclick="deleteMetodo('${m.id}')" style="margin-left:10px; color:red;">X</button>
        `;
        elements.listaMetodosPago.appendChild(li);
    });
}

async function handleMetodoSubmit(e) {
    e.preventDefault();
    const nombre = document.getElementById('metodoNombre').value;
    const tipo = document.getElementById('metodoTipo').value;

    try {
        const res = await fetch(API.metodosPago.base, {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify({ nombre, tipoPago: tipo })
        });
        if (res.ok) {
            document.getElementById('formularioMetodoPago').reset();
            loadMetodosPagoList();
        } else {
            alert("Error guardando método");
        }
    } catch (err) { console.error(err); }
}

window.deleteMetodo = async (id) => {
    if (!confirm("Borrar método?")) return;
    await fetch(API.metodosPago.byId(id), { method: 'DELETE', headers: getAuthHeaders() });
    loadMetodosPagoList();
};


/************************************************************
 * PERFIL USUARIO
 ************************************************************/
async function openModalPerfil() {
    try {
        const res = await fetch(API.usuarios.perfil, { headers: getAuthHeaders() });
        const user = await res.json();
        state.currentUser = user; // Actualizar state

        document.getElementById('perfilNombre').value = user.nombre;
        document.getElementById('perfilEmail').value = user.email;
        document.getElementById('perfilPassword').value = '';

        elements.modalPerfil.showModal();
    } catch (e) { alert("Error cargando perfil"); }
}

async function handlePerfilSubmit(e) {
    e.preventDefault();
    const nombre = document.getElementById('perfilNombre').value;
    const password = document.getElementById('perfilPassword').value;

    const dto = {
        nombre: nombre,
        email: state.currentUser.email,
        password: password ? password : state.currentUser.passwordHash // Backend debería manejar null password
    };

    // Nota: El backend espera 'UsuarioRequestDTO' que pide password. 
    // Si la contraseña está vacía, el frontend no debería enviarla o el backend ignorarla.
    // Asumiremos que enviamos el DTO y el backend hash si viene algo.
    if (!password) delete dto.password; // Si tu backend soporta esto. Si no, requiere lógica extra.

    try {
        const res = await fetch(API.usuarios.perfil, {
            method: 'PUT',
            headers: getAuthHeaders(),
            body: JSON.stringify(dto)
        });
        if (res.ok) {
            alert("Perfil actualizado");
            elements.modalPerfil.close();
        } else alert("Error actualizando perfil");
    } catch (e) { console.error(e); }
}


/************************************************************
 * EVENT LISTENERS GLOBALES
 ************************************************************/
function setupEventListeners() {
    // Header
    elements.btnCerrarSesion.addEventListener('click', handleLogout);
    elements.btnAbrirPerfil.addEventListener('click', openModalPerfil);
    elements.btnAbrirReportes.addEventListener('click', () => elements.modalReportes.showModal());
    elements.btnGestionarMetodos.addEventListener('click', openModalMetodos);

    // Gastos
    elements.btnAgregarGasto.addEventListener('click', () => openModalGasto(null));
    elements.formularioGasto.addEventListener('submit', handleGastoSubmit);

    // Categorias
    elements.btnAgregarCategoria.addEventListener('click', openModalCategoria);

    // Filtros
    elements.btnAbrirFiltros.addEventListener('click', async () => {
        await populateSelects();
        elements.modalFiltros.showModal();
    });

    elements.btnLimpiarFiltros.addEventListener('click', () => {
        elements.formularioFiltros.reset();
        loadGastos(); // Recarga sin filtros
        elements.modalFiltros.close();
    });

    elements.btnAplicarFiltros.addEventListener('click', (e) => {
        e.preventDefault();
        const filtro = {
            contieneString: document.getElementById('filtroBusqueda').value || null,
            fechaInicio: document.getElementById('filtroFechaInicio').value || null,
            fechaFin: document.getElementById('filtroFechaFin').value || null,
            categoriaId: elements.filtroCategoria.value || null,
            metodoDePagoId: elements.filtroMetodo.value || null
        };
        loadGastos(filtro);
        elements.modalFiltros.close();
    });

    // Reportes
    elements.formularioReportes.addEventListener('submit', handleExportarReporte);
    elements.formularioImportar.addEventListener('submit', handleImportarReporte);

    // Metodos Pago
    elements.formularioMetodoPago.addEventListener('submit', handleMetodoSubmit);

    // Perfil
    elements.formularioPerfil.addEventListener('submit', handlePerfilSubmit);

    // Cerrar Modales
    elements.botonesCerrarModal.forEach(btn => {
        btn.addEventListener('click', () => {
            // Encuentra el dialog padre y ciérralo
            btn.closest('dialog').close();
        });
    });
}