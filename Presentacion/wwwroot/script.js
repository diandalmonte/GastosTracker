/* ==========================================================================
   ESTADO DE LA APLICACIÓN (STATE)
   ========================================================================== */
const state = {
    token: localStorage.getItem('authToken') || null,
    isLoginMode: true,
    currentUser: null,
    // Variables para manejar edición/visualización
    currentDetailId: null,
    currentDetailType: null, // 'gasto' | 'categoria'
    currentGasto: null, // Objeto completo para edición
};

/* ==========================================================================
   ELEMENTOS DEL DOM (ELEMENTS)
   ========================================================================== */
const elements = {
    // Vistas Principales
    loginView: document.getElementById('loginView'),
    contenedorDashboard: document.getElementById('contenedorDashboard'),
    encabezadoPrincipal: document.getElementById('encabezadoPrincipal'),
    navegacionPrincipal: document.getElementById('navegacionPrincipal'),

    // Autenticación
    authForm: document.getElementById('authForm'),
    loginTitle: document.getElementById('loginTitle'),
    fieldNombre: document.getElementById('fieldNombre'),
    authNombre: document.getElementById('authNombre'),
    authEmail: document.getElementById('authEmail'),
    authPassword: document.getElementById('authPassword'),
    btnAuthAction: document.getElementById('btnAuthAction'),
    toggleAuthMode: document.getElementById('toggleAuthMode'),
    btnCerrarSesion: document.getElementById('btnCerrarSesion'),

    // Presupuesto (Sidebar)
    medidorPresupuesto: document.getElementById('medidorPresupuesto'),
    textoPorcentajePresupuesto: document.getElementById('textoPorcentajePresupuesto'),
    mostrarTotalGastado: document.getElementById('mostrarTotalGastado'),
    mostrarPresupuestoTotal: document.getElementById('mostrarPresupuestoTotal'),
    estadoPresupuesto: document.getElementById('estadoPresupuesto'),

    // Botones Header
    btnAbrirPerfil: document.getElementById('AbrirPerfil'),
    btnAbrirReportes: document.getElementById('btnAbrirReportes'),
    btnGestionarMetodos: document.getElementById('btnGestionarMetodos'),

    // Gastos (Tabla y Acciones)
    tablaGastos: document.getElementById('tablaGastos'),
    cuerpoTablaGastos: document.getElementById('cuerpoTablaGastos'),
    btnAgregarGasto: document.getElementById('btnAgregarGasto'),

    // Categorías
    contenedorListaCategorias: document.getElementById('contenedorListaCategorias'),
    btnAgregarCategoria: document.getElementById('btnAgregarCategoria'),

    // Modal Detalle Genérico
    modalDetalleGenerico: document.getElementById('modalDetalleGenerico'),
    tituloDetalleModal: document.getElementById('tituloDetalleModal'),
    cuerpoDetalleModal: document.getElementById('cuerpoDetalleModal'),
    btnActualizarDetalle: document.getElementById('btnActualizarDetalle'),
    btnEliminarDetalle: document.getElementById('btnEliminarDetalle'),

    // Modal Formulario Gasto
    modalFormularioGasto: document.getElementById('modalFormularioGasto'),
    formularioGasto: document.getElementById('formularioGasto'),
    gastoId: document.getElementById('gastoId'),
    gastoEncabezado: document.getElementById('gastoEncabezado'),
    gastoMonto: document.getElementById('gastoMonto'),
    gastoCategoria: document.getElementById('gastoCategoria'),
    gastoMetodo: document.getElementById('gastoMetodo'),
    gastoFecha: document.getElementById('gastoFecha'),
    gastoDescripcion: document.getElementById('gastoDescripcion'),
    tituloModalGasto: document.getElementById('tituloModalGasto'),

    // Modal Perfil
    modalPerfil: document.getElementById('modalPerfil'),
    formularioPerfil: document.getElementById('formularioPerfil'),
    perfilNombre: document.getElementById('perfilNombre'),
    perfilEmail: document.getElementById('perfilEmail'),
    perfilPassword: document.getElementById('perfilPassword'),

    // Modal Reportes
    modalReportes: document.getElementById('modalReportes'),
    formularioReportes: document.getElementById('formularioReportes'),
    formatoReporte: document.getElementById('formatoReporte'),
    formularioImportar: document.getElementById('formularioImportar'),
    archivoImportar: document.getElementById('archivoImportar'),

    // Modal Métodos de Pago
    modalMetodosPago: document.getElementById('modalMetodosPago'),
    listaMetodosPago: document.getElementById('listaMetodosPago'),
    formularioMetodoPago: document.getElementById('formularioMetodoPago'),
    metodoId: document.getElementById('metodoId'),
    metodoNombre: document.getElementById('metodoNombre'),
    metodoTipo: document.getElementById('metodoTipo'),

    // Filtros
    btnAbrirFiltros: document.getElementById('btnAbrirFiltros'),
    modalFiltros: document.getElementById('modalFiltros'),
    formularioFiltros: document.getElementById('formularioFiltros'),
    btnLimpiarFiltros: document.getElementById('btnLimpiarFiltros'),
    btnAplicarFiltros: document.getElementById('btnAplicarFiltros'),
    filtroBusqueda: document.getElementById('filtroBusqueda'),
    filtroFechaInicio: document.getElementById('filtroFechaInicio'),
    filtroFechaFin: document.getElementById('filtroFechaFin'),
    filtroCategoria: document.getElementById('filtroCategoria'),
    filtroMetodo: document.getElementById('filtroMetodo')
};

/* ==========================================================================
   API ENDPOINTS
   ========================================================================== */
const API = {
    auth: {
        login: '/api/auth/login',
        register: '/api/auth/register'
    },
    usuarios: {
        perfil: '/api/usuarios/perfil'
    },
    presupuesto: {
        general: '/api/presupuestos/general',
        porcentaje: '/api/presupuestos/porcentaje-general',
        diferencia: '/api/presupuestos/diferencia-general',
        resumenCategorias: '/api/presupuestos/resumen-categorias'
    },
    gastos: {
        base: '/api/gastos',
        usuario: '/api/gastos/usuario/', // Ojo con el slash final según tu controller
        byId: (id) => `/api/gastos/${id}`,
        buscar: '/api/gastos/buscar'
    },
    categorias: {
        base: '/api/categorias',
        byId: (id) => `/api/categorias/${id}`
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

/* ==========================================================================
   UTILIDADES
   ========================================================================== */
function getAuthHeaders() {
    return {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${state.token}`
    };
}

function getAuthHeadersMultipart() {
    // Para subir archivos NO definimos Content-Type, el navegador lo pone con el boundary
    return {
        'Authorization': `Bearer ${state.token}`
    };
}

function formatCurrency(amount) {
    return new Intl.NumberFormat('es-DO', { style: 'currency', currency: 'DOP' }).format(amount);
}

function formatDate(dateString) {
    if (!dateString) return '-';
    // dateString viene como "2025-05-10" (DateOnly serializado)
    return dateString;
}

/* ==========================================================================
   INICIALIZACIÓN
   ========================================================================== */
document.addEventListener('DOMContentLoaded', function () {
    setupEventListeners();

    if (state.token) {
        initDashboard();
    } else {
        showLoginView();
    }
});

function setupEventListeners() {
    // Autenticación
    elements.authForm.addEventListener('submit', handleAuth);
    elements.toggleAuthMode.addEventListener('click', toggleAuthMode);
    elements.btnCerrarSesion.addEventListener('click', handleLogout);

    // Navegación / Modales Superiores
    elements.btnAbrirPerfil.addEventListener('click', openProfileModal);
    elements.btnAbrirReportes.addEventListener('click', openReportModal);
    elements.btnGestionarMetodos.addEventListener('click', openMethodsModal);

    // Acciones de Gastos
    elements.btnAgregarGasto.addEventListener('click', () => openGastoModal()); // Crear
    elements.formularioGasto.addEventListener('submit', handleGastoSubmit);

    // Acciones de Categorías
    elements.btnAgregarCategoria.addEventListener('click', handleCreateCategoria);

    // Acciones de Detalle Genérico
    elements.btnActualizarDetalle.addEventListener('click', handleUpdateFromDetail);
    elements.btnEliminarDetalle.addEventListener('click', handleDeleteFromDetail);

    // Acciones de Métodos de Pago
    elements.formularioMetodoPago.addEventListener('submit', handleMetodoSubmit);

    // Acciones de Perfil
    elements.formularioPerfil.addEventListener('submit', handleProfileUpdate);

    // Acciones de Reportes (Importar/Exportar)
    elements.formularioReportes.addEventListener('submit', handleExport);
    elements.formularioImportar.addEventListener('submit', handleImport);

    // Filtros
    elements.btnAbrirFiltros.addEventListener('click', () => openModal(elements.modalFiltros));
    elements.btnLimpiarFiltros.addEventListener('click', () => {
        elements.formularioFiltros.reset();
        loadGastos(); // Recargar sin filtros
        closeModal(elements.modalFiltros);
    });
    elements.btnAplicarFiltros.addEventListener('click', handleFiltrosSubmit);

    // Cerrar Modales Global
    document.querySelectorAll('.btnCerrarModal').forEach(btn => {
        btn.addEventListener('click', (e) => {
            const modal = e.target.closest('dialog');
            closeModal(modal);
        });
    });
}

/* ==========================================================================
   LÓGICA DE VISTAS (LOGIN VS DASHBOARD)
   ========================================================================== */
function showLoginView() {
    elements.encabezadoPrincipal.classList.add('hidden');
    elements.contenedorDashboard.classList.add('hidden');
    elements.navegacionPrincipal.style.display = 'none';
    elements.loginView.classList.remove('hidden');// Ocultar menú
}

function showDashboardView() {
    elements.loginView.classList.add('hidden');
    elements.encabezadoPrincipal.classList.remove('hidden');
    elements.contenedorDashboard.classList.remove('hidden');
    elements.navegacionPrincipal.style.display = 'block'; // Mostrar menú
}

async function initDashboard() {
    showDashboardView();
    await Promise.all([
        loadPresupuestoData(),
        loadGastos(),
        loadCategoriasList()
    ]);
}

/* ==========================================================================
   AUTENTICACIÓN
   ========================================================================== */
function toggleAuthMode(e) {
    e.preventDefault();
    state.isLoginMode = !state.isLoginMode;

    if (state.isLoginMode) {
        elements.loginTitle.textContent = 'Iniciar Sesión';
        elements.btnAuthAction.textContent = 'Entrar';
        elements.toggleAuthMode.textContent = '¿No tienes cuenta? Regístrate';
        elements.fieldNombre.hidden = true;
        elements.authNombre.required = false;
    } else {
        elements.loginTitle.textContent = 'Registrarse';
        elements.btnAuthAction.textContent = 'Registrarme';
        elements.toggleAuthMode.textContent = '¿Ya tienes cuenta? Inicia Sesión';
        elements.fieldNombre.hidden = false;
        elements.authNombre.required = true;
    }
}

async function handleAuth(e) {
    e.preventDefault();

    const email = elements.authEmail.value;
    const password = elements.authPassword.value;
    const nombre = elements.authNombre.value;

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

        const data = await response.json();

        if (!response.ok) throw new Error(data.message || 'Error en autenticación');

        // Guardar token
        if (data.token) {
            state.token = data.token;
            localStorage.setItem('authToken', state.token);
            initDashboard();
        }

    } catch (error) {
        alert(error.message);
    }
}

function handleLogout() {
    state.token = null;
    state.currentUser = null;
    localStorage.removeItem('authToken');
    location.reload(); // Recargar para limpiar todo
}

/* ==========================================================================
   PRESUPUESTO
   ========================================================================== */
async function loadPresupuestoData() {
    try {
        // Obtenemos Presupuesto General y Diferencia
        const [resGeneral, resDiferencia, resPorcentaje] = await Promise.all([
            fetch(API.presupuesto.general, { headers: getAuthHeaders() }),
            fetch(API.presupuesto.diferencia, { headers: getAuthHeaders() }),
            fetch(API.presupuesto.porcentaje, { headers: getAuthHeaders() })
        ]);

        if (resGeneral.ok && resDiferencia.ok) {
            const presupuestoTotal = await resGeneral.json();
            const diferencia = await resDiferencia.json();
            const porcentaje = await resPorcentaje.json();

            // Calculamos el total gastado en base a la diferencia
            const totalGastado = presupuestoTotal - diferencia;

            elements.mostrarPresupuestoTotal.textContent = formatCurrency(presupuestoTotal);
            elements.mostrarTotalGastado.textContent = formatCurrency(totalGastado);

            // Meter
            elements.medidorPresupuesto.value = porcentaje;
            elements.textoPorcentajePresupuesto.textContent = `${porcentaje}%`;

            // Estado textual
            if (diferencia < 0) {
                elements.estadoPresupuesto.textContent = "¡Presupuesto Excedido!";
                elements.estadoPresupuesto.style.color = "red";
            } else {
                elements.estadoPresupuesto.textContent = "Dentro del presupuesto";
                elements.estadoPresupuesto.style.color = "green";
            }
        }
    } catch (error) {
        console.error("Error cargando presupuesto:", error);
    }
}

/* ==========================================================================
   GASTOS (Listado y CRUD)
   ========================================================================== */
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
        console.error("Error cargando gastos:", error);
    }
}

function renderTablaGastos(gastos) {
    elements.cuerpoTablaGastos.innerHTML = '';
    gastos.forEach(gasto => {
        // GastoVistaPrevia llega aquí
        const row = document.createElement('tr');
        row.innerHTML = `
            <td>-</td> <td>${gasto.nombreCategoria}</td>
            <td>${formatCurrency(gasto.monto)}</td>
            <td>-</td> <td>
                <button class="btn-ver-detalle" data-id="${gasto.id}">Ver</button>
            </td>
        `;

        // Listener para abrir detalle completo
        row.querySelector('.btn-ver-detalle').addEventListener('click', () => showGastoDetalle(gasto.id));
        elements.cuerpoTablaGastos.appendChild(row);
    });
}

// Abrir Modal Crear/Editar Gasto
async function openGastoModal(gastoEditar = null) {
    await loadSelectOptions(); // Cargar categorías y métodos en los selects

    if (gastoEditar) {
        state.currentGasto = gastoEditar;
        elements.tituloModalGasto.textContent = "Editar Gasto";
        elements.gastoId.value = gastoEditar.id;
        elements.gastoEncabezado.value = gastoEditar.encabezado;
        elements.gastoMonto.value = gastoEditar.monto;
        elements.gastoCategoria.value = gastoEditar.categoriaId || ''; // Asumiendo DTO tiene IDs
        elements.gastoMetodo.value = gastoEditar.metodoDePagoId || '';
        elements.gastoFecha.value = gastoEditar.fecha;
        elements.gastoDescripcion.value = gastoEditar.descripcion || '';
    } else {
        state.currentGasto = null;
        elements.tituloModalGasto.textContent = "Registrar Gasto";
        elements.formularioGasto.reset();
        elements.gastoId.value = '';
        // Setear fecha hoy por defecto
        elements.gastoFecha.value = new Date().toISOString().split('T')[0];
    }
    openModal(elements.modalFormularioGasto);
}

// Guardar Gasto (Create / Update)
async function handleGastoSubmit(e) {
    e.preventDefault();

    const dto = {
        encabezado: elements.gastoEncabezado.value,
        monto: parseFloat(elements.gastoMonto.value),
        categoriaId: elements.gastoCategoria.value,
        metodoDePagoId: elements.gastoMetodo.value,
        fecha: elements.gastoFecha.value, // "YYYY-MM-DD" DateOnly lo acepta
        descripcion: elements.gastoDescripcion.value,
        isFechaActual: false // Usamos la fecha del input
    };

    const id = elements.gastoId.value;
    const method = id ? 'PUT' : 'POST';
    const url = id ? API.gastos.byId(id) : API.gastos.base;

    if (id) dto.id = id;

    try {
        const response = await fetch(url, {
            method: method,
            headers: getAuthHeaders(),
            body: JSON.stringify(dto)
        });

        if (response.ok) {
            const data = id ? null : await response.json(); // POST devuelve alertas
            if (data && data.alertas && data.alertas.length > 0) {
                alert("Gasto guardado. ALERTAS DE PRESUPUESTO:\n" + data.alertas.join("\n"));
            }
            closeModal(elements.modalFormularioGasto);
            closeModal(elements.modalDetalleGenerico); // Por si veníamos de detalle
            initDashboard(); // Recargar todo
        } else {
            const err = await response.text();
            alert("Error al guardar: " + err);
        }
    } catch (error) {
        console.error(error);
        alert("Error de red.");
    }
}

// Ver Detalle Completo de un Gasto
async function showGastoDetalle(id) {
    try {
        const response = await fetch(API.gastos.byId(id), { headers: getAuthHeaders() });
        if (response.ok) {
            const gasto = await response.json();
            // Guardar estado para acciones
            state.currentDetailId = gasto.id;
            state.currentDetailType = 'gasto';
            state.currentGasto = gasto; // Guardamos el objeto completo (tiene nombres y IDs?)
            // Nota: El endpoint GetGasto devuelve GastoReadDTO (Strings), no IDs. 
            // Para editar necesitamos IDs. El frontend podría necesitar inferirlos o el backend enviarlos.
            // Asumiremos que para editar usaremos lo que hay, o si faltan IDs, habría que ajustar el backend.
            // * Ajuste rápido: El ReadDTO no tiene IDs de Categoria/Metodo. 
            // * Solución ideal: Backend envía IDs en ReadDTO. 
            // * Solución "parche": Al abrir editar, el usuario tendrá que re-seleccionar si no coinciden.

            renderDetalleModal("Detalle de Gasto", [
                { label: "Encabezado", value: gasto.encabezado },
                { label: "Monto", value: formatCurrency(gasto.monto) },
                { label: "Categoría", value: gasto.nombreCategoria },
                { label: "Método", value: gasto.metodoDePago },
                { label: "Fecha", value: formatDate(gasto.fecha) },
                { label: "Descripción", value: gasto.descripcion || "N/A" }
            ]);
        }
    } catch (error) {
        console.error(error);
    }
}

/* ==========================================================================
   CATEGORÍAS
   ========================================================================== */
async function loadCategoriasList() {
    try {
        // Usamos el endpoint de resumen que trae datos procesados (excedido, %, etc)
        const response = await fetch(API.presupuesto.resumenCategorias, { headers: getAuthHeaders() });
        if (response.ok) {
            const categorias = await response.json();
            renderListaCategorias(categorias);
        }
    } catch (error) {
        console.error(error);
    }
}

function renderListaCategorias(categorias) {
    elements.contenedorListaCategorias.innerHTML = '';

    // Convertimos a array si no lo es
    const lista = Array.isArray(categorias) ? categorias : [];

    lista.forEach(cat => {
        const div = document.createElement('div');
        div.className = `categoria-card ${cat.isExcedido ? 'excedido' : ''}`;
        div.innerHTML = `
            <h4>${cat.nombre}</h4>
            <p>Presupuesto: ${formatCurrency(cat.montoPresupuesto)}</p>
            <p>Uso: ${cat.porcentajePresupuesto}%</p>
        `;
        div.addEventListener('click', () => showCategoriaDetalle(cat.id));
        elements.contenedorListaCategorias.appendChild(div);
    });
}

async function handleCreateCategoria() {
    const nombre = prompt("Nombre de la nueva categoría:");
    if (!nombre) return;

    const presupuestoStr = prompt("Monto del presupuesto mensual:");
    if (!presupuestoStr) return;

    const monto = parseFloat(presupuestoStr);
    if (isNaN(monto) || monto < 0) {
        alert("Monto inválido");
        return;
    }

    try {
        const response = await fetch(API.categorias.base, {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify({ nombre, montoPresupuesto: monto })
        });

        if (response.ok) {
            initDashboard();
        } else {
            const msg = await response.text();
            alert("Error: " + msg);
        }
    } catch (error) {
        alert("Error de conexión");
    }
}

async function showCategoriaDetalle(id) {
    try {
        const response = await fetch(API.categorias.byId(id), { headers: getAuthHeaders() });
        if (response.ok) {
            const cat = await response.json();
            state.currentDetailId = cat.id;
            state.currentDetailType = 'categoria';
            state.currentGasto = null;

            renderDetalleModal("Detalle de Categoría", [
                { label: "Nombre", value: cat.nombre },
                { label: "Presupuesto", value: formatCurrency(cat.montoPresupuesto) },
                { label: "Estado", value: cat.isExcedido ? "Excedido" : "OK" }
            ]);
        }
    } catch (error) {
        console.error(error);
    }
}

/* ==========================================================================
   MÉTODOS DE PAGO
   ========================================================================== */
async function openMethodsModal() {
    await loadMethodsList();
    openModal(elements.modalMetodosPago);
}

async function loadMethodsList() {
    try {
        const response = await fetch(API.metodosPago.base, { headers: getAuthHeaders() });
        if (response.ok) {
            const metodos = await response.json();
            elements.listaMetodosPago.innerHTML = '';
            metodos.forEach(m => {
                const li = document.createElement('li');
                li.innerHTML = `
                    <span>${m.nombre} (${m.tipoPago})</span>
                    <button class="boton-peligro-sm" onclick="deleteMetodo('${m.id}')">X</button>
                `;
                elements.listaMetodosPago.appendChild(li);
            });
        }
    } catch (error) {
        console.error(error);
    }
}

// Función global para poder llamarla desde el HTML inyectado
window.deleteMetodo = async function (id) {
    if (!confirm("¿Eliminar este método?")) return;
    try {
        await fetch(API.metodosPago.byId(id), { method: 'DELETE', headers: getAuthHeaders() });
        loadMethodsList();
    } catch (e) { alert(e); }
};

async function handleMetodoSubmit(e) {
    e.preventDefault();
    const nombre = elements.metodoNombre.value;
    const tipo = elements.metodoTipo.value;

    try {
        const response = await fetch(API.metodosPago.base, {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify({ nombre, tipoPago: tipo })
        });
        if (response.ok) {
            elements.formularioMetodoPago.reset();
            loadMethodsList();
        } else {
            alert(await response.text());
        }
    } catch (e) { alert(e); }
}

/* ==========================================================================
   PERFIL
   ========================================================================== */
async function openProfileModal() {
    try {
        const response = await fetch(API.usuarios.perfil, { headers: getAuthHeaders() });
        if (response.ok) {
            const user = await response.json();
            state.currentUser = user;
            elements.perfilNombre.value = user.nombre;
            elements.perfilEmail.value = user.email;
            elements.perfilPassword.value = '';
            openModal(elements.modalPerfil);
        }
    } catch (e) { console.error(e); }
}

async function handleProfileUpdate(e) {
    e.preventDefault();
    const nombre = elements.perfilNombre.value;
    const pass = elements.perfilPassword.value;

    const body = { nombre, email: state.currentUser.email, password: pass || "dummyPass" };
    // Nota: El DTO UsuarioRequestDTO pide password obligatorio según el modelo C#, 
    // pero si está vacío no deberíamos cambiarlo. El backend debería manejar eso.
    // Asumiremos que enviamos el pass si existe, sino el backend debe validar.
    // Si el backend es estricto, habría que enviar el pass actual o cambiar lógica.

    try {
        const response = await fetch(API.usuarios.perfil, {
            method: 'PUT',
            headers: getAuthHeaders(),
            body: JSON.stringify(body)
        });
        if (response.ok) {
            alert("Perfil actualizado");
            closeModal(elements.modalPerfil);
        } else {
            alert("Error actualizando perfil");
        }
    } catch (e) { alert(e); }
}

/* ==========================================================================
   REPORTES (Import/Export)
   ========================================================================== */
function openReportModal() {
    openModal(elements.modalReportes);
}

async function handleExport(e) {
    e.preventDefault();
    const formato = elements.formatoReporte.value;
    try {
        // Fetch como Blob para descargar
        const response = await fetch(API.reportes.exportar(formato), { headers: getAuthHeaders() });
        if (response.ok) {
            const blob = await response.blob();
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            // Intentar sacar nombre del header o generarlo
            a.download = `Reporte_${new Date().getTime()}.${formato === 'Excel' ? 'xlsx' : formato.toLowerCase()}`;
            document.body.appendChild(a);
            a.click();
            a.remove();
        } else {
            alert("Error generando reporte");
        }
    } catch (e) { alert(e); }
}

async function handleImport(e) {
    e.preventDefault();
    const file = elements.archivoImportar.files[0];
    if (!file) return;

    const formData = new FormData();
    formData.append('archivo', file);

    try {
        const response = await fetch(API.reportes.importar, {
            method: 'POST',
            headers: getAuthHeadersMultipart(),
            body: formData
        });

        if (response.ok) {
            alert("Importación exitosa");
            initDashboard();
            closeModal(elements.modalReportes);
        } else {
            alert("Error al importar: " + await response.text());
        }
    } catch (e) { alert(e); }
}

/* ==========================================================================
   FILTROS
   ========================================================================== */
async function handleFiltrosSubmit(e) {
    e.preventDefault();

    // Cargar options si no están cargadas
    if (elements.filtroCategoria.options.length <= 1) await loadSelectOptions(true);

    const filtro = {
        contieneString: elements.filtroBusqueda.value || null,
        fechaInicio: elements.filtroFechaInicio.value || null,
        fechaFin: elements.filtroFechaFin.value || null,
        categoriaId: elements.filtroCategoria.value || null,
        metodoDePagoId: elements.filtroMetodo.value || null
    };

    await loadGastos(filtro);
    closeModal(elements.modalFiltros);
}

/* ==========================================================================
   MODAL GENÉRICO Y HELPERS
   ========================================================================== */
function renderDetalleModal(titulo, campos) {
    elements.tituloDetalleModal.textContent = titulo;
    elements.cuerpoDetalleModal.innerHTML = '';

    const dl = document.createElement('dl');
    campos.forEach(campo => {
        dl.innerHTML += `<dt><strong>${campo.label}:</strong></dt><dd>${campo.value}</dd>`;
    });
    elements.cuerpoDetalleModal.appendChild(dl);

    openModal(elements.modalDetalleGenerico);
}

async function handleUpdateFromDetail() {
    if (state.currentDetailType === 'gasto') {
        // Necesitamos cargar los datos completos del gasto primero para editarlo bien
        // Ya lo tenemos en showGastoDetalle? Si, pero necesitamos mapear al formulario.
        // Hacemos fetch again o usamos state.currentGasto si tiene todo
        // El endpoint byId devuelve DTO de lectura (Strings), para editar necesitamos IDs.
        // Intentaremos abrir el modal, el usuario deberá rellenar selects si no coinciden.
        try {
            // Nota: Para una UX perfecta, el backend debería devolver IDs en el ReadDTO
            // Aquí haremos un "best effort".
            openGastoModal(state.currentGasto);
        } catch (e) { console.error(e); }
    } else if (state.currentDetailType === 'categoria') {
        const nuevoNombre = prompt("Nuevo Nombre:");
        const nuevoPres = prompt("Nuevo Presupuesto:");
        if (nuevoNombre && nuevoPres) {
            try {
                const response = await fetch(API.categorias.byId(state.currentDetailId), {
                    method: 'PUT',
                    headers: getAuthHeaders(),
                    body: JSON.stringify({ id: state.currentDetailId, nombre: nuevoNombre, montoPresupuesto: parseFloat(nuevoPres) })
                });
                if (response.ok) {
                    initDashboard();
                    closeModal(elements.modalDetalleGenerico);
                } else alert(await response.text());
            } catch (e) { alert(e); }
        }
    }
}

async function handleDeleteFromDetail() {
    if (!confirm("¿Seguro que deseas eliminar este elemento?")) return;

    let url;
    if (state.currentDetailType === 'gasto') url = API.gastos.byId(state.currentDetailId);
    else if (state.currentDetailType === 'categoria') url = API.categorias.byId(state.currentDetailId);

    try {
        const response = await fetch(url, { method: 'DELETE', headers: getAuthHeaders() });
        if (response.ok || response.status === 204) {
            closeModal(elements.modalDetalleGenerico);
            initDashboard();
        } else {
            alert(await response.text());
        }
    } catch (e) { alert(e); }
}

async function loadSelectOptions(forFilters = false) {
    // Cargar Categorias
    try {
        const resCat = await fetch(API.categorias.base, { headers: getAuthHeaders() });
        const cats = await resCat.json();

        // Cargar Métodos
        const resMet = await fetch(API.metodosPago.base, { headers: getAuthHeaders() });
        const mets = await resMet.json();

        const selectCat = forFilters ? elements.filtroCategoria : elements.gastoCategoria;
        const selectMet = forFilters ? elements.filtroMetodo : elements.gastoMetodo;

        // Guardar valor actual si existe
        const currentCatVal = selectCat.value;
        const currentMetVal = selectMet.value;

        selectCat.innerHTML = `<option value="">${forFilters ? 'Todas' : 'Seleccione...'}</option>`;
        cats.forEach(c => selectCat.innerHTML += `<option value="${c.id}">${c.nombre}</option>`);

        selectMet.innerHTML = `<option value="">${forFilters ? 'Todos' : 'Seleccione...'}</option>`;
        mets.forEach(m => selectMet.innerHTML += `<option value="${m.id}">${m.nombre}</option>`);

        // Restaurar valor si aun existe
        if (currentCatVal) selectCat.value = currentCatVal;
        if (currentMetVal) selectMet.value = currentMetVal;
    } catch (e) { alert("Primero defina las categorias: " + e); }
    
}

function openModal(modal) {
    modal.classList.remove('hidden');
    modal.showModal();
}

function closeModal(modal) {
    modal.close();
    modal.classList.add('hidden');
}