export default function validate(form, formType = 0) {
    // 0 là form default
    const inputs = form.querySelectorAll("input[data-vtype]")
    let isValid = false

    // Xử lí khi submit form
    form.addEventListener("submit", function (e) {
        if (!isValid)
            e.preventDefault()
        let isFormValid = true

        for (let input of inputs) {
            let types = input.dataset.vtype.split("-")
            let val = input.value
            let isInputValid = true

            let errElm = document.createElement("span")
            errElm.classList.add("err-msg")
            let inputContainer = input.closest("div")
            let errMsg = ""

            for (let type of types) {
                if (!isInputValid)
                    break

                let maxl, minl

                if (type.includes("MinLength")) {
                    minl = parseInt(type.split("_")[1])
                    type = "MinLength"
                }

                if (type.includes("MaxLength")) {
                    minl = parseInt(type.split("_")[1])
                    type = "MaxLength"
                }

                switch (type) {
                    case "Required":
                        if (val == "") {
                            isInputValid = false
                            errMsg = "Trường này là bắt buộc"
                        }
                        break
                    case "NotNumber":
                        if (val != "")                            
                            for (let c of val)
                                if (!isNaN(c) && c != " ") {
                                    console.log(isNaN(c))
                                    console.log(c)
                                    isInputValid = false
                                    errMsg = "Vui lòng không nhập số"
                                    break;
                                }
                        break
                    case "Mail":
                        if (!val.match(/\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b/)) {
                            isInputValid = false
                            errMsg = "Email không hợp lệ"
                        }
                        break
                    case "MinLength":
                        if (val.length < minl) {
                            isInputValid = false
                            errMsg = "Phải nhập ít nhất " + minl + " ký tự"
                        }
                        break
                    case "MaxLength":
                        if (val.length > maxl) {
                            isInputValid = false
                            errMsg = "Không nhập quá " + maxl + " ký tự"
                        }
                        break
                    case "Distinct":
                        break 
                    case "Phone":
                        if (val != "")                            
                            if (!val.match(/\b0[0-9]{9}\b/)) {
                                isInputValid = false
                                errMsg = "Số điện thoại không hợp lệ"
                            }
                        break
                    case "CCCD":
                        if (!val.match(/\b0[0-9]{11}\b/)) {
                            isInputValid = false
                            errMsg = "Số CCCD không hợp lệ"
                        }
                        break
                }
            }

            if (!isInputValid) {
                inputContainer.classList.add("err")
                if (inputContainer.querySelector(".err-msg") == null)
                    inputContainer.appendChild(errElm)
                errElm.textContent = errMsg
            }

            isFormValid = isFormValid && isInputValid
        }

        if (isFormValid) {
            isValid = true
            if (formType == 0)
                form.submit()
        }
    })

    // Hàm xoá trạng thái lỗi
    const clearErrState = (input) => { 
        let inputContainer = input.closest("div")
        let errMsg = inputContainer.querySelector(".err-msg")
        if (errMsg)
            errMsg.remove()
        inputContainer.classList.remove("err")
    }

    // Lặp qua các input để bỏ trạng thái lỗi khi focus hoặc nhập vào input
    for (let input of inputs) {
        input.addEventListener("change", function (e) {
            clearErrState(input)
        })
        input.addEventListener("focus", function (e) {
            clearErrState(input)
        })
    }

    // lắng nghe hành vi đóng modal chứa form để loại bỏ các trạng thái lỗi
    let closeBtn = form.querySelector("button[type=reset]")
    let modalContainer = form.closest("#createModal")
    let modal = form.closest(".modal-dialog")

    if (closeBtn)
        closeBtn.addEventListener("click", function (e) {
            for (let input of inputs) 
                clearErrState(input)
        })

    if (modalContainer) {
        modalContainer.addEventListener("click", function (e) {
            console.log(modal)
            for (let input of inputs)
                clearErrState(input)
        })
        modal.addEventListener("click", function (e) {
            e.stopPropagation()            
        })
    }
        
}